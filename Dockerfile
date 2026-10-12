FROM mcr.microsoft.com/dotnet/sdk:10.0 as base
WORKDIR /src
COPY ["blog.csproj", "./"]
RUN dotnet restore "blog.csproj"
COPY . .
RUN dotnet publish "blog.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app
COPY --from=base /app/publish .
EXPOSE 8080
ENTRYPOINT [ "dotnet", "blog.dll" ]

