namespace SearchAndSort;

/**
 * <summary>
 * An immutable object which is returned by <see cref="Road.GetData"/>. This is filled by the <see cref="DataProvider"/>
 * and <see cref="OrderState"/> provided by the developer.
 *
 * This DataResult stores a copy of the road data and can be manipulated by the sorts without issue, as the Road
 * stores an unsorted copy of the array. This makes it safe to edit the data array supplied by the DataResult
 * safely.
 * 
 * </summary>
 */
public class DataResult {

    private readonly DataProvider dataProvider;
    public OrderState orderState { get; private set; } // Should only be set by the Flip() function
    private readonly int[] data;
    
    /**
     * <summary>
     * Constructs an instance from the Road Data, <see cref="DataProvider"/> and <see cref="OrderState"/> supplied.
     * </summary>
     */
    public DataResult(int[] rawData, DataProvider dataProvider, OrderState orderState) {
        this.dataProvider = dataProvider;
        this.orderState = orderState;

        switch (orderState) {
            case OrderState.Ascending:
                data = dataProvider.sort.Ascending(rawData);
                break;
            case OrderState.Descending:
            default:    
                data = dataProvider.sort.Ascending(rawData);
                break;
        }
    }

    /**
     * <summary>
     * Searches the Data and returns an <see cref="ExactDataPosition"/> of all indexes found, or empty if none.
     * </summary>
     *
     * <returns>An instance of <see cref="ExactDataPosition"/> with the indexes found</returns>
     */
    public ExactDataPosition GetDataPositionExact(int number) {
        return new ExactDataPosition(number, dataProvider.search.FindIndexes(this, data, number));
    }

    /**
     * <summary>
     * Searches the Data and returns the <see cref="NearestDataPosition"/> of all indexes found.
     * </summary>
     *
     * <returns>An instance of <see cref="NearestDataPosition"/> with the indexes found</returns>
     */
    public NearestDataPosition GetNearestDataPosition(int number) {
        return new NearestDataPosition(number, dataProvider.search.FindNearestNumbersAndIndex(this, data, number));
    }

    /**
     * <summary>
     * Reverses the Data into the opposite data
     * </summary>
     */
    public void Reverse() {
        dataProvider.sort.Flip(data);
        orderState = orderState == OrderState.Ascending ? OrderState.Descending : OrderState.Ascending;
    }

    /**
     * <summary>
     * Prints the Index and Value of the sorted data
     * </summary>
     */
    public void PrintData() {
        for (var i = 0; i < data.Length; i++) {
            Util.Print($"Index {i}; Val {data[i]}");
        }
    }

    /**
     * <summary>
     * Prints the Index and Value of the sorted data, skipping every step amount of values
     * </summary>
     */
    public void PrintData(int step) {
        for (var i = 0; i < data.Length; i += step) {
            Util.Print($"Index {i}; Val {data[i]}");
        }
    }

}

