namespace Common.DbContext
{
    public class DbContextProvider : IDbContextProvider
    {
        public DataContext Get()
        {
            return new DataContext();
        }
    }
}