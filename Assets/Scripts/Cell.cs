using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Item item;
    public Vector2 position;
    public Cell(Item item, Vector2 position)
    {
        this.item = item;
        this.position = position;
    }
}
