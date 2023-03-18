namespace SearchAndSort;

/**
 * Immutable!!!
 */
public class DataResult {

    private readonly DataProvider dataProvider;
    public readonly OrderState orderState;
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
        return new ExactDataPosition(number, dataProvider.search.FindIndexes(this, data, number));
    }

    public NearestDataPosition GetNearestDataPosition(int number) {
        return new NearestDataPosition(number, dataProvider.search.FindNearestNumbersAndIndex(this, data, number));
    }

    public DataResult Reverse() {
        dataProvider.sort.Flip(data);
        return this;
        
        return new DataResult(
            dataProvider.sort.Flip(data),
            dataProvider,
            orderState == OrderState.Ascending ? OrderState.Descending : OrderState.Ascending
        );
    }

    public void PrintData() {
        for (int i = 0; i < data.Length; i++) {
            Util.Print($"Index {i}; Val {data[i]}");
        }
    }

    public void PrintData(int step) {
        for (int i = 0; i < data.Length; i += step) {
            Util.Print($"Index {i}; Val {data[i]}");
        }
    }

}

