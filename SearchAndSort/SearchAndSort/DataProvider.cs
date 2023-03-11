namespace SearchAndSort; 

public class DataProvider {

    public readonly ISearch search;
    public readonly ISort sort;
    
    private DataProvider(ISearch search, ISort sort) {
        this.search = search;
        this.sort = sort;
    }

    public static DataProvider Of(ISearch search, ISort sort) {
        return new DataProvider(search, sort);
    }
    
}
