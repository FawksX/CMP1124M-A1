namespace SearchAndSort;

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
public class NameAttribute : System.Attribute {
    public string name;

    public NameAttribute(string name) {
        this.name = name;
    }
}

public interface ISort {
    int[] Descending(int[] data);

    int[] Ascending(int[] data) {
        return Flip(Descending(data));
    }

    /**
     * Assumes it is already sorted.
     */
    int[] Flip(int[] data) {
        var descendingLength = data.Length;
        var reverse = new int[descendingLength];
        var length = descendingLength - 1;

        for (var i = 0; i < descendingLength; i++) {
            reverse[i] = data[length];
            length--;
        }

        return reverse;
    }

    string GetName() {
        var type = typeof(NameAttribute);
        NameAttribute attribute = (NameAttribute)Attribute.GetCustomAttribute(type, type);

        if (attribute != null) {
            return attribute.name;
        }

        Console.WriteLine($"ERROR: Sort {GetType().Name} is missing Name Attribute!");
        return "";
    }
}

[Name("Bubble Sort")]
public class BubbleSort : ISort {
    public int[] Descending(int[] data) {
        var length = data.Length;

        for (var i = 1; i < length - 1; i++) {
            for (var j = 0; j < length - 1; j++) {
                if (data[j] > data[j + 1]) {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]);
                }
            }
        }

        return data;
    }
}

[Name("Insertion Sort")]
public class InsertionSort : ISort {
    // todo
    public int[] Descending(int[] data) {
        var length = data.Length;

        for (var index = 0; index < length; index++) {
            var current = data[index];
            var previous = index - 1;

            while (previous >= 0 && current < data[previous]) {
                data[previous + 1] = data[previous];
                data[previous] = current;
                previous--;
            }
        }

        return data;
    }
}

[Name("Merge Sort")]
public class MergeSort : ISort {
    
    /** We're embedding this into another method just to reduce the repetitiveness a bit.
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
     **/
    
    public int[] Descending(int[] data) {
        return Sort(data.ToList()).ToArray();
    }

    private List<int> Sort(List<int> data) {
        var length = data.Count;
        if (length <= 1) {
            return data;
        }

        var left = new List<int>();
        var right = new List<int>();

        var middle = length / 2;

        for (var index = 0; index < middle; index++) {
            left.Add(data[index]);
        }

        for (var index = middle; index < length; index++) {
            right.Add(data[index]);
        }

        left = Sort(left);
        right = Sort(right);

        var merge = new List<int>();
        while (left.Count > 0 || right.Count > 0) {

            var leftCount = left.Count;
            var rightCount = right.Count;

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

[Name("Cocktail Shaker Sort")]
public class CocktailShakerSort : ISort {
    // todo
    public int[] Descending(int[] data) {

        var swapped = false;
        var start = 0;
        var length = data.Length;

        while (swapped) {
            swapped = false;

            for (var i = start; i < length; i++) {
                if (data[i] > data[i + 1]) {
                    (data[i], data[i + 1]) = (data[i + 1], data[i]);
                    swapped = true;
                }
            }

            if (!swapped) {
                break;
            }

            swapped = false;
            length--;

            for (var i = length - 1; i >= start; i--) {
                if (data[i] > data[i + 1]) {
                    (data[i], data[i + 1]) = (data[i + 1], data[i]);
                    swapped = true;
                }
            }

            start++;

        }

        return data;

    }
}

public class Sorts {
    public static readonly BubbleSort BUBBLE_SORT = new BubbleSort();
    public static readonly InsertionSort INSERTION_SORT = new InsertionSort();
    public static readonly MergeSort MERGE_SORT = new MergeSort();
    public static readonly CocktailShakerSort COCKTAIL_SHAKER_SORT = new CocktailShakerSort();
}