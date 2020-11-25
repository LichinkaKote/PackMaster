using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Item item;
    public Vector2Int position;
    public InventoryUI inventoryUI;
    private bool disabled;

    public bool Disabled { get => disabled; }

    public Cell(Item item, Vector2Int position)
    {
        this.item = item;
        this.position = position;
    }
    public void SetColor(Color color)
    {
        inventoryUI.SetItemColor(this, color);
    }
    public void Disable(bool value)
    {
        inventoryUI.DisableItemDrag(this, value);
        disabled = value;
    }
}
