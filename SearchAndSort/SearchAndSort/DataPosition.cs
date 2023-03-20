namespace SearchAndSort; 

/**
 * <summary>
 * An abstract container object which may or may not contain the indexes of a request.
 * If a value is present, <see cref="isPresent"/> will return <code>true</code> and
 * <see cref="Result"/> will return the values (or the (int, List(int) ) for nearest indexes).
 * </summary>
 *
 * <seealso cref="ExactDataPosition"/>
 * <seealso cref="NearestDataPosition"/>
 */
public abstract class AbstractDataPosition<T> {

    public readonly int request;
    private readonly bool isPresent;

    /**
     * <summary>
     * Constructs an instance with the request and if the result is present
     * </summary>
     */
    protected AbstractDataPosition(int request, bool isPresent) {
        this.request = request;
        this.isPresent = isPresent;
    }

    /**
     * <summary>
     * Return <code>false</code> if there is a value present, otherwise <code>true</code>
     * </summary>
     * 
     * <returns>
     * <code>false</code> if there is a value present, otherwise <code>true</code>
     * </returns>
     */
    public bool IsEmpty() {
        return !isPresent;
    }

    /**
     * <summary>
     * Return <code>true</code> if there is a value present, otherwise <code>false</code>
     * </summary>
     * 
     * <returns>
     * <code>true</code> if there is a value present, otherwise <code>false</code>
     * </returns>
     */
    public bool IsPresent() {
        return isPresent;
    }

    /**
     * <summary>
     * Return the result of the data positions if present
     * </summary>
     * 
     * <returns>The result of the data positions if present</returns>
     */
    public abstract T Result();

}

/**
 * <summary>
 * An implementation of <see cref="AbstractDataPosition{T}"/> for when the number requested has been found
 * </summary>
 */
public class ExactDataPosition : AbstractDataPosition<List<int>> {

    private List<int> indexes;
    
    public ExactDataPosition(int number, List<int> indexes) : base(number, indexes.Count > 0) {
        this.indexes = indexes;
    }

    public override string ToString() {
        return string.Join(",", indexes);
    }

    public override List<int> Result() {
        return indexes;
    }

}

/**
 * <summary>
 * An implementation of <see cref="AbstractDataPosition{T}"/> for when the number requested could not be
 * found, and the next nearest number (with its indexes) is found.
 * </summary>
 */
public class NearestDataPosition : AbstractDataPosition<(int, List<int>)> {

    private (int, List<int>) numbers2IndexList;
    
    public NearestDataPosition(int number, (int, List<int>) numbers2IndexList) : 
        base(number, numbers2IndexList.Item2.Count > 0) {
        this.numbers2IndexList = numbers2IndexList;
    }

    public override string ToString() {
        return $"num;{numbers2IndexList.Item1}, indexes;{string.Join(",", numbers2IndexList.Item2)}";
    }

    public override (int, List<int>) Result() {
        return numbers2IndexList;
    }
}