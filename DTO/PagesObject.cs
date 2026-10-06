namespace Blog.DTO
{
    public class PagesObject<T>
    {
        public List<T> Values {get;}
        public int TotalObjects {get;}
        public int Page {get;}
        public int SizePage {get;}

        public PagesObject(List<T> val, int totalObjects, int page, int sizePage)
        {
            Values = val;
            TotalObjects = totalObjects;
            Page = page;
            SizePage = sizePage;
        }


        
    }
}