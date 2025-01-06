namespace Catalog.Core.Specs;

public class Pagination<T> where T : class
{
    public Pagination()
    {
        
    }
    
    public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
    {
        
    }
}