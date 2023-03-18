
namespace SearchAndSort;

public class Road {

    private static readonly string ROAD_DIRECTORY = "data/";
    
    private readonly string? path;
    private int[] rawData;
    
    private Road(string? path, int[]? rawData) {
        this.path = path;

        if (rawData == null && path != null) {
            this.rawData = PopulateData();
        } else if (rawData != null) {
            this.rawData = rawData;
        } else {
            this.rawData = Array.Empty<int>();
        }
    }

    private int[] PopulateData() {
        var list = new List<int>();
        try {
            var data = File.ReadAllLines(ROAD_DIRECTORY + path);
            
            foreach (var s in data) {
                list.Add(Convert.ToInt32(s));
            }
            return list.ToArray();
        }
        catch (Exception exception) {
            Util.Print($"Exception Caught when loading Road {path} - {exception.Message}");
        }

        return list.ToArray();
    }

    public DataResult GetData(DataProvider provider, OrderState orderState) {
        return new DataResult(rawData, provider, orderState);
    }

    public static RoadBuilder Builder(string path) {
        return new RoadBuilder(path);
    }

    public static RoadBuilder Builder() {
        return new RoadBuilder();
    }

    public class RoadBuilder {

        private string? path;
        private int[]? rawData;

        public RoadBuilder(string path) {
            this.path = path;
        }

        public RoadBuilder() {
            
        }

        public RoadBuilder PopulateData(int[] rawData) {
            this.rawData = rawData;
            return this;
        }

        public RoadBuilder MergeRoad(Road road) {

            List<int> data = new List<int>();
            if (rawData != null) {
                data.AddRange(rawData);
            }

            data.AddRange(road.rawData);
            rawData = data.ToArray();
            return this;
        }

        public Road Build() {
            return new Road(path, rawData);
        }
        
    }
    
}

public class Roads {

    public static readonly Road ROAD_1_256 = Initialise("Road_1_256.txt");
    public static readonly Road ROAD_1_2048 = Initialise("Road_1_2048.txt");
    public static readonly Road ROAD_2_256 = Initialise("Road_2_256.txt");
    public static readonly Road ROAD_2_2048 = Initialise("Road_2_2048.txt");
    public static readonly Road ROAD_3_256 = Initialise("Road_3_256.txt");
    public static readonly Road ROAD_3_2048 = Initialise("Road_3_2048.txt");

    private static Road Initialise(string path) {
        return Road.Builder(path).Build();
    }

    public static Road? FromMagic(int number) {
        return number switch {
            1 => ROAD_1_256,
            2 => ROAD_2_256,
            3 => ROAD_3_256,
            4 => ROAD_1_2048,
            5 => ROAD_2_2048,
            6 => ROAD_3_2048
        };
    }
    
}