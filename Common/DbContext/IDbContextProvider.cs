namespace Common.DbContext
{
    public interface IDbContextProvider
    {
        DataContext Get();
    }
}