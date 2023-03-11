namespace SearchAndSort; 

public abstract class AbstractDataPosition {

    public readonly int request;
    private readonly bool isPresent;

    protected AbstractDataPosition(int request, bool isPresent) {
        this.request = request;
        this.isPresent = isPresent;
    }

    public bool IsEmpty() {
        return !isPresent;
    }

    public bool IsPresent() {
        return isPresent;
    }
    
}

public class ExactDataPosition : AbstractDataPosition {

    private List<int> indexes;

    public ExactDataPosition(int number, List<int> indexes) : base(number, indexes.Count > 0) {
        this.indexes = indexes;
    }

    public override string ToString() {
        return String.Join(",", indexes);
    }
    
}

public class NearestDataPosition : AbstractDataPosition {

    private (int, List<int>) numbers2IndexList;
    
    public NearestDataPosition(int number, (int, List<int>) numbers2IndexList) : 
        base(number, numbers2IndexList.Item2.Count > 0) {
        this.numbers2IndexList = numbers2IndexList;
    }

    public override string ToString() {
        return $"num;{numbers2IndexList.Item1}, indexes;{String.Join(",", numbers2IndexList.Item2)}";
    }
}