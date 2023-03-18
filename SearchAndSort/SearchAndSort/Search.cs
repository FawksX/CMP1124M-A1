namespace SearchAndSort;

public interface ISearch {

    List<int> FindIndexes(DataResult dataResult, int[] data, int number);

    // Number Found : Index
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
        
        list.Add(foundIndex);

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

public class Search {

    public static readonly BinarySearch BINARY_SEARCH = new BinarySearch();
    public static readonly LinearSearch LINEAR_SEARCH = new LinearSearch();

}