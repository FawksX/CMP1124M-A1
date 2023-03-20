namespace SearchAndSort;

/**
 * <summary>
 * ISearch Interface
 * Acts as an abstraction over any search implemented in this project, meaning we can abstract all other search code
 * throughout the project.
 * </summary>
 */
public interface ISearch {

    /**
     * <summary>
     * Finds all indexes for the requested number
     * </summary>
     * <returns>
     * Returns a List of indexes, which could be empty if no indexes are found.
     * </returns>
     */
    List<int> FindIndexes(DataResult dataResult, int[] data, int number);

    /**
     * <summary>
     * Finds the nearest number with indexes to the requested number
     * </summary>
     * <returns>
     * Returns a Tuple with the nearest number (Item1) and it's indexes (Item2)
     * </returns>
     */
    (int, List<int>) FindNearestNumbersAndIndex(DataResult dataResult, int[] data, int number) {

        var stepOut = 0;

        while (true) {
            var numbersLeft = FindIndexes(dataResult, data, number - stepOut);
            var numbersRight = FindIndexes(dataResult, data, number + stepOut);

            if (numbersLeft.Count != 0) {
                return (number - stepOut, numbersLeft);
            }
            
            if (numbersRight.Count != 0) {
                return (number + stepOut, numbersRight);
            }

            stepOut++;
        }

    }

}

/**
 * <summary>
 * A Simple Binary Search
 * O(log N) "Divide and Conquer" based searching algorithm where the list is continuously split in half
 * until the value is found.
 * </summary>
 */
public class BinarySearch : ISearch {

    public List<int> FindIndexes(DataResult dataResult, int[] data, int number) {

        var found = false;
        var low = 0;
        var high = data.Length - 1;

        var foundIndex = 0;
        
        while (low <= high && !found) {
            var mid = (low + high) / 2;
            if (data[mid] == number) {
                found = true;
                foundIndex = mid;
            } else if (data[mid] < number) {
                // Depending on the ordering of the array, we need to do different operations, so check before
                // sorting:
                if (dataResult.orderState == OrderState.Ascending) {
                    low = mid + 1;
                } else {
                    high = mid - 1;
                }
                
            } else {
                if (dataResult.orderState == OrderState.Ascending) {
                    high = mid - 1;
                } else {
                    low = mid + 1;
                }
            }
        }

        var list = new List<int>();
        if (!found) {
            return list;
        }
        
        // This is the middle value, we now need to find every other value
        list.Add(foundIndex);

        // So we add the middle value to the list, then go from this index left and right
        // to find every other missing value (as the list is ordered, the first index which isn't what we're looking for
        // is the end of that side.
        
        // check left side first:
        var foundAllLeft = false;
        high = foundIndex - 1;
        while (!foundAllLeft) {
            if (data[high] == number) {
                list.Add(data[high]);
                high--;
            }

            if (data[high] != number) {
                foundAllLeft = true;
            }
        }
        
        // check right side second:
        var foundAllRight = false;
        high = foundIndex + 1;
        while (!foundAllRight) {
            if (data[high] == number) {
                list.Add(data[high]);
                high++;
            }

            if (data[high] != number) {
                foundAllRight = true;
            }
        }

        return list;
    }
    
}

/**
 * <summary>
 * Linear Search
 * O(N) Algorithm which iterates the array until the first index is found, and then keeps iterating until all elements have
 * been found
 * </summary>
 */
public class LinearSearch : ISearch {
    
    public List<int> FindIndexes(DataResult dataResult, int[] data, int number) {

        var indexes = new List<int>();
        var found = false;
        
        for (var i = 0; i < data.Length; i++) {

            if (data[i] == number) {
                indexes.Add(i);
                found = true;
            }
            
            // If we have already found one in a sorted list, the first element which isn't
            // the number we're after means we have found all the indexes (in a linear search anyway).
            if (found && data[i] != number) {
                return indexes; // all elements have been found
            }
        }

        return indexes;
    }
    
}

/**
 * <summary>
 * A class which stores our Binary and Linear Search Singletons.
 * </summary>
 */
public class Search {

    public static readonly BinarySearch BINARY_SEARCH = new BinarySearch();
    public static readonly LinearSearch LINEAR_SEARCH = new LinearSearch();

}