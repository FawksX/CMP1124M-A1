namespace SearchAndSort;

public class Testing {
    private static readonly string NEW_LINE = "\n";

    public static void Init() {
        Functionality2("Road 1 256", Roads.ROAD_1_256, 10, Search.BINARY_SEARCH, Sorts.BUBBLE_SORT);
        Functionality2("Road 2 256", Roads.ROAD_2_256, 10, Sorts.MERGE_SORT);
        Functionality2("Road 3 256", Roads.ROAD_3_256, 10, Sorts.INSERTION_SORT);

        Functionality3And4();

        Util.Print("Beginning Functionality 5: Repeat Tasks 2-4");
        Functionality2("Road 1 2048", Roads.ROAD_1_2048, 50, Sorts.COCKTAIL_SHAKER_SORT);
        Functionality2("Road 2 2048", Roads.ROAD_2_2048, 50);
        Functionality2("Road 3 2048", Roads.ROAD_3_2048, 50);
        
        Util.Print("Functionality 6:");
        Functionality6And7("Road 1 (256) & Road 3 (256) - Merge", 10, Roads.ROAD_1_256, Roads.ROAD_3_256);
        Functionality6And7("Road 1 (2048) & Road 3 (2048) - Merge", 50, Roads.ROAD_1_2048, Roads.ROAD_3_2048);
    }
    
    private static void Functionality2(string name, Road road, int step, ISort sort) =>
        Functionality2(name, road, step, Search.LINEAR_SEARCH, sort);
    
    private static void Functionality2(string name, Road road, int step) =>
        Functionality2(name, road, step, Search.LINEAR_SEARCH, Sorts.BUBBLE_SORT);
    
    private static void Functionality2(string name, Road road, int step, ISearch search, ISort sort) {
        Util.Print($"Functioality 2: {name}");
        DataProvider dataProvider = DataProvider.Of(search, sort);
        DataResult result = road.GetData(dataProvider, OrderState.Descending);

        Util.Print($"Printing Data Descending (every 10 values) for road {name}");
        result.PrintData(step);

        result.Reverse();
        Util.Print($"Printing Data Ascending (every 10 values) for road {name}");
        result.PrintData(step);
    }

    private static void Functionality3And4() {
        Util.Print("Functionality 3 & 4: Finding Nearest Values");

        while (true) {
            try {
                Util.Print("Please Select One of the Following Roads:" +
                           $"{NEW_LINE}1. Road 1 (length 256)" +
                           $"{NEW_LINE}2. Road 2 (length 256)" +
                           $"{NEW_LINE}3. Road 3 (length 256)" +
                           $"{NEW_LINE}4. Road 1 (length 2048)" +
                           $"{NEW_LINE}5. Road 2 (length 2048)" +
                           $"{NEW_LINE}6. Road 3 (length 2048): "
                );

                var number = int.Parse(Console.ReadLine());
                var road = Roads.FromMagic(number);

                if (road == null) {
                    Util.Print("The road inputted could not be found! Restarting...");
                    continue;
                }

                if (Functionality3And4_Secondary(road)) {
                    return;
                }
            }
            catch (Exception exception) {
                Util.Print("You must input integers when using this method!");
            }
        }
    }

    private static bool Functionality3And4_Secondary(Road road) =>
        Functionality3And4_Secondary(road, Search.LINEAR_SEARCH);
    private static bool Functionality3And4_Secondary(Road road, ISearch search) {
        try {
            Util.Print("What number would you like to find in this road? ");
            var number2 = int.Parse(Console.ReadLine());

            var dataProvider = DataProvider.Of(search, Sorts.BUBBLE_SORT);
            var data = road.GetData(dataProvider, OrderState.Descending);

            var position = data.GetDataPositionExact(number2);
            if (position.IsEmpty()) {
                var nearest = data.GetNearestDataPosition(number2);
                Util.Print($"Data Position for {position.request} does not exist! The Nearest Number is {nearest.Result().Item1} with indexes {string.Join(",", nearest.Result().Item2)}");
                return true;
            }

            Util.Print($"The Data Positions for {position.request} are {position}");
            return true;
        }
        catch (Exception exception) {
            Util.Print("You must input integers when using this method!");
        }
        return false;
    }

    private static void Functionality6And7(string name, int step, params Road[] roads) {

        var roadBuilder = Road.Builder();
        foreach (var road in roads) {
            roadBuilder.MergeRoad(road);
        }

        var mergedRoad = roadBuilder.Build();

        Functionality2(name, mergedRoad, step);
        Functionality3And4_Secondary(mergedRoad, Search.BINARY_SEARCH);
    }
}