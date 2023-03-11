namespace SearchAndSort {


    class Program {

        static void Main(string[] args) {

            DataProvider dataProvider = DataProvider.Of(Search.LINEAR_SEARCH, Sorts.BUBBLE_SORT);
            DataResult data = Roads.ROAD_1_256.GetData(dataProvider, OrderState.Descending);

            ExactDataPosition position = data.GetDataPositionExact(10);
            if (position.IsEmpty()) {
                NearestDataPosition nearest = data.GetNearestDataPosition(10);
                Util.Print($"Data Position for {position.request} does not exist! The Nearest Positions are {nearest}");
                return;
            }
            
            Util.Print($"The Data Positions for {position.request} are {position}");

        }
        
    }

}