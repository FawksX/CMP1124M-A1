
namespace SearchAndSort;

/**
 * <summary>
 * Road Object
 * A road is defined using the <see cref="RoadBuilder"/> - This applies to both ways a road is added into the spec:
 * - Via file - such as Road_1_256.txt
 * - Via code - Merging Roads, custom data, etc.
 * </summary>
 */
public class Road {

    private static readonly string ROAD_DIRECTORY = "data/";
    
    private readonly string? path;
    private int[] rawData;
    
    /**
     * <summary>
     * Constructs an instance from the result of the <see cref="RoadBuilder"/> - Both the path and data can be null
     * in this instance to make an empty road.
     *
     * If the Road is being loaded from a file, only the rawData will be null.
     * </summary>
     */
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

    /**
     * <summary>
     * Populates the Road with data loaded from the File. This is discovered from the <see cref="path"/> above.
     * </summary>
     *
     * <returns>The Int Array loaded from the file</returns>
     */
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

    /**
     * <summary>
     * Creates a DataResult from the provided <see cref="DataProvider"/> and <see cref="OrderState"/>
     * This contains an ordered array and allows for datapoints to be found.
     * </summary>
     * <returns>The <see cref="DataResult"/> instance which was created</returns>
     */
    public DataResult GetData(DataProvider provider, OrderState orderState) {
        Util.Print($"Creating Road Data using {provider.sort.GetName()} Sorting Algorithm...");
        return new DataResult(rawData, provider, orderState);
    }

    /**
     * <summary>
     * Creates a new <see cref="RoadBuilder"/> prefilled with the path specified
     * </summary>
     * <returns>A new <see cref="RoadBuilder"/> instance</returns>
     */
    public static RoadBuilder Builder(string path) {
        return new RoadBuilder(path);
    }

    /**
     * <summary>
     * Creates a new <see cref="RoadBuilder"/>
     * </summary>
     * <returns>A new <see cref="RoadBuilder"/> instance</returns>
     */
    public static RoadBuilder Builder() {
        return new RoadBuilder();
    }

    /**
     * <summary>
     * RoadBuilder
     *
     * A simple Builder for creating our roads. This can be used to import from a file, or to merge roads together.
     * These can be instantiated using <see cref="Road.Builder()"/> and <see cref="Road.Builder(string)"/>
     * </summary>
     */
    public class RoadBuilder {

        private string? path;
        private int[]? rawData;

        /**
         * <summary>Constructs a RoadBuilder instance, prefilling the Path specified</summary>
         */
        public RoadBuilder(string path) {
            this.path = path;
        }

        /**
         * <summary>Constructs a RoadBuilder instance</summary>
         */
        public RoadBuilder() {
            
        }

        /**
         * <summary>
         * Populates the future road with the specified road
         * </summary>
         * <returns><see cref="RoadBuilder"/></returns>
         */
        public RoadBuilder PopulateData(int[] rawData) {
            this.rawData = rawData;
            return this;
        }

        /**
         * <summary>
         * Merges the specified <see cref="Road"/> data into the new road. This is useful for combining two roads' data
         * together. For example;
         * <code>
         * Road.Builder()
         *     .MergeRoad(Roads.ROAD_1_256)
         *     .MergeRoad(Roads.ROAD_2_256)
         *     .Build()
         * </code>
         * This will return the combined data of Road 1 (256) and Road 2 (256)
         * </summary>
         * <returns><see cref="RoadBuilder"/></returns>
         */
        public RoadBuilder MergeRoad(Road road) {

            List<int> data = new List<int>();
            if (rawData != null) {
                data.AddRange(rawData);
            }

            data.AddRange(road.rawData);
            rawData = data.ToArray();
            return this;
        }

        /**
         * <summary>
         * Builds the Road using the parameters specified
         * </summary>
         * <returns><see cref="Road"/></returns>
         */
        public Road Build() {
            return new Road(path, rawData);
        }
        
    }
    
}

/**
 * <summary>
 * Acts as a place for us to store Constants for our roads which are in file format for the
 * assignment. Other than this, there is a static method which assigns the roads magic values which is used within the
 * <see cref="Testing"/> class.
 </summary>
 */
public class Roads {

    public static readonly Road ROAD_1_256 = Initialise("Road_1_256.txt");
    public static readonly Road ROAD_1_2048 = Initialise("Road_1_2048.txt");
    public static readonly Road ROAD_2_256 = Initialise("Road_2_256.txt");
    public static readonly Road ROAD_2_2048 = Initialise("Road_2_2048.txt");
    public static readonly Road ROAD_3_256 = Initialise("Road_3_256.txt");
    public static readonly Road ROAD_3_2048 = Initialise("Road_3_2048.txt");

    /**
     * <summary>Initialises a Road from file using the builder</summary>
     * <returns><see cref="Road"/></returns>
     */
    private static Road Initialise(string path) {
        return Road.Builder(path).Build();
    }
    
    /**
     * <summary>Finds a road from an inputted magic value if it exists, else null</summary>
     * <returns><see cref="Road"/>, or null if not found.</returns>
     */
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