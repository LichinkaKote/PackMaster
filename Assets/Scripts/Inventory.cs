using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Observer
{
    private Vector2Int inventorySize = new Vector2Int(2, 2);
    public List<Cell> cells = new List<Cell>();

    public Vector2Int Size { get => inventorySize; set => inventorySize = value; }

    public void AddItem(Cell cell)
    {
        cells.Add(cell);
        Notify();
    }
    public void RemoveItem(Cell cell)
    {
        cells.Remove(cell);
        Notify();
    }
    public void MoveItem(Vector2Int position, Vector2Int newPosition)
    {
        if (TryGetCellWithPosition(position, out Cell cell))
        {
            if (IsPossibleToPlaceItem(newPosition, cell))
            {
                cell.position = newPosition;
            }
            Notify();
        }
    }
    public bool TryGetCellWithPosition(Vector2Int position, out Cell cell)
    {
        cell = null;
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].position == position)
            {
                cell = cells[i];
                return true;
            }
        }
        return false;
    }
    public bool IsPossibleToPlaceItem(Vector2Int targetPosition, Cell cell)
    {
        var itemSize = cell.item.Size;
        if (itemSize.x * itemSize.y > GetFreeCellsCount()) return false;
        if (!IsItemInBounds(targetPosition, itemSize)) return false;
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i] == cell) continue;

            var position = cells[i].position;
            var currentItemSize = cells[i].item.Size;
            if (IsItemsOverlap(position, currentItemSize, targetPosition, itemSize))
            {
                return false;
            }
        }
        return true;
    }
    public int GetFreeCellsCount()
    {
        int ocupied = 0;
        for (int i = 0; i < cells.Count; i++)
        {
            ocupied += cells[i].item.Size.x * cells[i].item.Size.y;
        }
        return Size.x * Size.y - ocupied;
    }
    private bool IsItemsOverlap(Vector2Int pos1, Vector2Int size1, Vector2Int pos2, Vector2Int size2)
    {
        bool b1 = pos2.x + (size2.x - 1) >= pos1.x;
        bool b2 = pos2.x <= pos1.x + (size1.x - 1);
        bool b3 = pos2.y + (size2.y - 1) >= pos1.y;
        bool b4 = pos2.y <= pos1.y + (size1.y - 1);
        //Debug.Log(pos1 + "" + pos2);
        //Debug.Log("b1="+ b1); Debug.Log("b2=" + b2); Debug.Log("b3=" + b3); Debug.Log("b4=" + b4);
        bool value = b1 && b2 && b3 && b4;
        //Debug.Log("overlap="+value);
        return value;
    }
    private bool IsItemInBounds(Vector2Int targetPosition, Vector2Int itemSize)
    {
        return (targetPosition.x >= 0 && targetPosition.y >= 0 &&
                targetPosition.x < inventorySize.x &&
                targetPosition.y < inventorySize.y &&
                targetPosition.x + itemSize.x <= inventorySize.x &&
                targetPosition.y + itemSize.y <= inventorySize.y);
    }
}
public struct FillParams
{
    public float fillPercent;
    public List<Item> posibleItems;
}

