namespace SearchAndSort; 

/**
 * <summary>
 * A wrapper class which provides a <see cref="ISearch"/> implementation  and a <see cref="ISort"/>
 * implementation. This class is immutable, and in actual implementation could allow for pairing
 * search and sort algorithms together (which may be useful depending on the size of the data being
 * sorted.
 * </summary>
 */
public class DataProvider {

    public readonly ISearch search;
    public readonly ISort sort;
    
    /**
     * <summary>
     * Constructs an instance using the supplied <see cref="ISearch"/> and <see cref="ISort"/> instances.
     * </summary>
     */
    private DataProvider(ISearch search, ISort sort) {
        this.search = search;
        this.sort = sort;
    }

    /**
     * <summary>
     * A static factory method which returns a new instance of <see cref="DataProvider"/> from the
     * inputted Search and Sort implementations
     * </summary>
     *
     * <returns>An immutable <see cref="DataProvider"/> associated with the supplied algorithms.</returns>
     */
    public static DataProvider Of(ISearch search, ISort sort) {
        return new DataProvider(search, sort);
    }
    
}
