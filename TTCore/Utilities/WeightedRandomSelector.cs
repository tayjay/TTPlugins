using System;
using System.Collections.Generic;
using System.Linq;

namespace TTCore.Utilities;

public class WeightedRandomSelector<T>
{
private List<WeightedItem<T>> items;
private double totalWeight;
private Random random;

public WeightedRandomSelector(IEnumerable<WeightedItem<T>> items)
{
    this.items = items.ToList();
    totalWeight = items.Sum(item => item.Weight);
    random = new Random();
}

public T SelectItem()
{
    double r = random.NextDouble() * totalWeight;
    foreach (var item in items)
    {
        if (r <= item.Weight)
            return item.Item;
        r -= item.Weight;
    }

    throw new InvalidOperationException("Should never reach here"); 
}
}