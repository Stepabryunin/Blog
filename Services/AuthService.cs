using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DevOne.Security.Cryptography.BCrypt;
using System.Text;
using Blog.Interfaces;
using Blog.Mapper;
using Blog.DTO;
using Blog.Entity;
namespace Blog.Services
{
    public class AuthService:IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly UserMapper _userMapper;

        public AuthService(IConfiguration congig, IUserService userService, IRefreshTokenService refreshTokenService, UserMapper userMapper)
        {

            _config=congig;
            _userService=userService;
            _refreshTokenService = refreshTokenService;
            _userMapper = userMapper;
        }
        public async Task<Result<AuthResult>> CreateUser(CreatedUser user)
        {
            var userEntity = new User(user.Username,user.Email,ToHashPassword(user.Password), RoleEnum.User);
            if (!await _userService.AddUserAsync(userEntity))
                return new Result<AuthResult>(ResultStatus.Conflict,null,"Email или Username уже занят");
            var accessToken = CreateAccessToken(userEntity.Id);
            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            string refreshTokenString  = await _refreshTokenService.CreateTokenAsync(userEntity.Id,DateTime.UtcNow.AddDays(7));
            var result = new AuthResult(_userMapper.ToFront(userEntity),accessTokenString,refreshTokenString);
            return new Result<AuthResult>(ResultStatus.Ok,result, null);
        }

        public async Task<Result<AuthResult>> Login(LoginUser user)
        {
            var userEntity = await _userService.GetUserByEmailAsync(user.Email);
            if (userEntity == null || !VerifyPassword(userEntity.PasswordHash, user.Password))
                return new Result<AuthResult>(ResultStatus.NotFound, null, "Неверный логин или пароль");
            var accessToken = CreateAccessToken(userEntity.Id);
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var refreshTokenString = await _refreshTokenService.CreateTokenAsync(userEntity.Id, DateTime.UtcNow.AddDays(7));
            var result = new AuthResult(_userMapper.ToFront(userEntity),accessTokenString,refreshTokenString);
            return new Result<AuthResult>(ResultStatus.Ok, result, null);
        }
        public async Task<Result<AuthResult>> Refresh(RefreshTokenDTO Token)
        {
            var token = await _refreshTokenService.RotateTokenAsync(Token.Token,DateTime.UtcNow.AddDays(7));
            if (token is null)
                return new Result<AuthResult>(ResultStatus.Unauthorized, null, "Токен истёк или не существует");
            var userEntity = await _userService.GetUserByIdAsync(token.UserId);
                        if (userEntity is null)
                 return new Result<AuthResult>(ResultStatus.Unauthorized, null, "Токен истёк или не существует");
            var accessToken = CreateAccessToken(token.UserId);
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            return new Result<AuthResult>(ResultStatus.Ok, new AuthResult(_userMapper.ToFront(userEntity),accessTokenString, token.RefreshToken), null);
        }

        public async Task<Result<bool>> Logout(Guid userId,string token)
        {
            bool res = await  _refreshTokenService.RevokeToken(userId, token);
            if (!res)
                return new Result<bool>(ResultStatus.Unauthorized,false,"Токен истёк");
            return new Result<bool>(ResultStatus.Ok,true, null);
        }
        private JwtSecurityToken CreateAccessToken(Guid userId)
        {
            var claims = new List<Claim>{new Claim(JwtRegisteredClaimNames.Sub,userId.ToString())};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:Key"]));
            var jwt = new JwtSecurityToken(
                issuer: _config["JwtOptions:Issuer"],
                audience: _config["JwtOptions:Audiencer"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );
            return jwt;
            
        }

        private string ToHashPassword(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password,workFactor:12);
        }
        private bool VerifyPassword(string hashPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password,hashPassword);
        }
    }
}