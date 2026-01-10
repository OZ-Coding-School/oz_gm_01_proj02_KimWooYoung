using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<(T grid, int priority)> elements = new List<(T grid, int priority)>(); 

    public int Count => elements.Count;

    public void Enqueue(T grid, int priority)
    {
        elements.Add((grid, priority));
    }
    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 1; i < elements.Count; i++)
        {
            if (elements[i].priority < elements[bestIndex].priority)
            {
                bestIndex = i;
            }
        }
        T bestItem = elements[bestIndex].grid;

        elements.RemoveAt(bestIndex);

        return bestItem;
    }
}
