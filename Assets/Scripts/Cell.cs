using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Item item;
    public Vector2Int position;
    public Cell(Item item, Vector2Int position)
    {
        this.item = item;
        this.position = position;
    }
}
