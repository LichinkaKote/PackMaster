using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Observer
{
    private Vector2Int size = new Vector2Int(2, 2);
    public List<Item> items = new List<Item>();

    public Vector2Int Size { get => size; set => size = value; }

    public void AddItem(Item item)
    {
        items.Add(item);
        Notify();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        Notify();
    }
}
