namespace SearchAndSort;

/**
 * <summary>
 * Abstract Sort Interface
 * As all Sorts will perform the same functions, we can abstract this so our code is cleaned throughout the project.
 * All Sorts should implement this interface to ensure no functionality is missing.
 * </summary>
 */
public interface ISort {
    int[] Descending(int[] data);

    int[] Ascending(int[] data) {
        return Flip(Descending(data));
    }

    /**
     * Assumes it is already sorted.
     */
    int[] Flip(int[] data) {
        var length = data.Length;

        for (var i = 0; i < length / 2; i++) {
            (data[i], data[length - i - 1]) = (data[length - i - 1], data[i]);
        }

        return data;
    }

    string GetName() {
        return GetType().Name;
    }
}

/**
 * <summary>
 * Bubble Sort Implementation
 * Bubble sort works by comparing all values in the list until it is in the right place, Here is an example;
 *
 * Lets assume we have the Array [9, 4, 5, 6, 4, 6, 2, 1]
 * We will compare the first two elements in the list, 9 and 4. They are in the wrong order, so swap
 * [4, 9, 5, 6, 4, 6, 2, 1]
 * We compare 9 with 5, They are in the wrong order, so swap
 * [4, 5, 9, 6, 4, 6, 2, 1]
 * ... and so on and so on, until this is done for all elements in the list. effectively "bubbling" the largest
 * numbers to the top.
 * </summary>
 */
public class BubbleSort : ISort {
    public int[] Descending(int[] data) {
        var length = data.Length;

        for (var i = 1; i < length - 1; i++) {
            for (var j = 0; j < length - 1; j++) {
                if (data[j] > data[j + 1]) {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]); // swap the two values shorthand
                }
            }
        }

        return data;
    }
}

/**
 * <summary>
 * Insertion Sort
 * This works by iterating through the list and "inserting" the value into the correct location on the list; for example:
 * [9, 4, 5, 6, 4, 6, 2, 1] - Initial list - the 9 is seen as sorted
 * first iteration, 4 is moved before the 9 - [4, 9, 5, 6, 4, 6, 2, 1]
 * second iteration, the 5 is placed between the 4 and 9 - [4, 5, 9, 6, 4, 6, 2, 1]
 * and so on - until the list is sorted.
 * </summary>
 */
public class InsertionSort : ISort {
    public int[] Descending(int[] data) {
        var length = data.Length;

        for (var index = 0; index < length; index++) {
            var current = data[index];
            var previous = index - 1;

            // Iterate and move the value down the list until the previous value is less than the one being inserted
            while (previous >= 0 && current < data[previous]) {
                data[previous + 1] = data[previous];
                data[previous] = current;
                previous--;
            }
        }

        return data;
    }
}

/**
 * <summary>Merge Sort
 * We're embedding this into another method just to reduce the repetitiveness a bit.
 * As this is a merge sort, we are basing it off this model:
 *
 * Lets assume we have the Array [9, 4, 5, 6, 4, 6, 2, 1]
 * We continuously split the array until it's single elements; ie:
 * [9, 4, 5, 6] [4, 6, 2, 1]
 * [9, 4], [5, 6], [4, 6], [2, 1]
 * [9], [4], [5], [6], [4], [6], [2], [1]
 *
 * Once at this state, we recombine and sort in-place; as such:
 * [4, 9], [5, 6], [4, 6], [1, 2]
 * [4, 5, 6, 9], [1, 2, 4, 6]
 * [1, 2, 4, 4, 5, 6, 6, 9]
 *
 * With this, we can iterate the same method over and over until the array has been split into
 * single-integer arrays, and then combine and merge in-place (done by checking left.First() <= right.First())
 * </summary>
**/
public class MergeSort : ISort {
    public int[] Descending(int[] data) {
        return Sort(data.ToList()).ToArray();
    }

    private List<int> Sort(List<int> data) {
        var length = data.Count;
        
        // As we keep using this method, this tells the program we have got to the state
        // where each variable is in it's own list, so just return the single element.
        if (length <= 1) {
            return data;
        }

        var left = new List<int>();
        var right = new List<int>();

        var middle = length / 2;

        // Split the arrays in half
        for (var index = 0; index < middle; index++) {
            left.Add(data[index]);
        }

        for (var index = middle; index < length; index++) {
            right.Add(data[index]);
        }

        // sort the two halves (or keep splitting them)
        left = Sort(left);
        right = Sort(right);

        var merge = new List<int>();
        while (left.Count > 0 || right.Count > 0) {
            var leftCount = left.Count;
            var rightCount = right.Count;

            // Put the cards in the right order
            if (leftCount > 0 && rightCount > 0) {
                if (left.First() <= right.First()) {
                    handle(merge, left);
                    continue;
                }

                handle(merge, right);
            }
            else if (leftCount > 0) {
                handle(merge, left);
            }
            else if (rightCount > 0) {
                handle(merge, right);
            }
        }

        return merge;
    }

    private void handle(List<int> merge, List<int> remove) {
        merge.Add(remove.First());
        remove.Remove(remove.First());
    }
}

/**
 * <summary>
 * A CockTail Shaker Sort is a Bi-directional bubble sort and pushes values to both the top
 * and bottom of the array. For example, once it has passed a value from the bottom to the top of the list,
 * it will then work backwards and push a value as far down as neccesary.
 * </summary>
 */
public class CocktailShakerSort : ISort {
    public int[] Descending(int[] data) {
        var swapped = true;
        var start = 0;
        var length = data.Length - 1;

        while (swapped) {
            swapped = false;

            // start to end loop
            for (var i = start; i < length; i++) {
                swapped = AttemptSwap(data, i);
            }

            // sorted
            if (!swapped) return data;

            swapped = false;
            length--; // Reduce the length by one, as we just sorted an element above

            // end to start
            for (var i = length - 1; i >= start; i--) {
                swapped = AttemptSwap(data, i);
            }

            // we sorted the value at the bottom, so we want the next iteration to start one step in
            start++;
        }

        return data;
    }

    private bool AttemptSwap(int[] data, int index) {
        if (!(data[index] > data[index + 1])) {
            return false;
        }
        Swap(data, index, index + 1);
        return true;
    }
    
    private void Swap(int[] data, int index1, int index2) {
        (data[index1], data[index2]) = (data[index2], data[index1]);
    }
}

public class Sorts {
    public static readonly BubbleSort BUBBLE_SORT = new BubbleSort();
    public static readonly InsertionSort INSERTION_SORT = new InsertionSort();
    public static readonly MergeSort MERGE_SORT = new MergeSort();
    public static readonly CocktailShakerSort COCKTAIL_SHAKER_SORT = new CocktailShakerSort();
}