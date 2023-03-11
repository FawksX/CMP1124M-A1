namespace SearchAndSort;

/**
 * Immutable!!!
 */
public class DataResult {

    private readonly DataProvider dataProvider;
    private readonly OrderState orderState;
    private readonly int[] data;
    
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

    public ExactDataPosition GetDataPositionExact(int number) {
        return new ExactDataPosition(number, dataProvider.search.FindIndexes(data, number));
    }

    public NearestDataPosition GetNearestDataPosition(int number) {
        return new NearestDataPosition(number, dataProvider.search.FindNearestNumbersAndIndex(data, number));
    }

    public DataResult Reverse() {
        return new DataResult(
            dataProvider.sort.Flip(data),
            dataProvider,
            orderState == OrderState.Ascending ? OrderState.Descending : OrderState.Ascending
        );
    }
    
}

