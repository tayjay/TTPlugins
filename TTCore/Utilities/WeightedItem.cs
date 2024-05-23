namespace TTCore.Utilities;

public class WeightedItem<T>
{
    public T Item { get; }
    public double Weight { get; }

    public WeightedItem(T item, double weight)
    {
        Item = item;
        Weight = weight;
    }
}