using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoObserver
{
    [SerializeField] private GameObject itemPF, gridPf;
    [SerializeField] private RectTransform content;
    private GameObject[,] itemObjects;

    private Inventory inventory = new Inventory();
    private Vector2 cellSize;

    public Inventory Inventory { get => inventory; }
    public Vector2 CellSize { get => cellSize; }
    public RectTransform ContentRect { get => content; }
    public RectTransform Rect { get => content.parent as RectTransform; }

    public static InventoryUI Create(int sizeX, int sizeY)
    {
        var inv = new Inventory();
        inv.Size = new Vector2Int(sizeX, sizeY);
        var inst = Instantiate(Prefabs.inventoryUI);
        var UI = inst.GetComponent<InventoryUI>();
        UI.SetInventory(inv);
        return UI;
    }
    private void Awake()
    {
        observable = inventory;
        cellSize = new Vector2(100, 100);
        CalculateContentSize();
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        CalculateContentSize();
        Resubscribe(inventory);
        DrawCellGrid();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        DrawCellGrid();
    }
    public override void Notify()
    {
        DrawCellGrid();
    }

    public void SetCellSize(Vector2 cellSize)
    {
        this.cellSize = cellSize;
        CalculateContentSize();
        DrawCellGrid();
    }
    public void SetPosition(Vector2 position)
    {
        content.anchoredPosition = position;
    }
    private void ClearContent()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    private void CalculateContentSize()
    {
        var rect = content.transform as RectTransform;
        rect.sizeDelta = new Vector2(cellSize.x * inventory.Size.x, cellSize.y * inventory.Size.y);
    }

    public void DrawCellGrid()
    {
        ClearContent();
        GameObject inst;
        for (int x = 0; x < inventory.Size.x; x++)
        {
            for (int y = 0; y < inventory.Size.y; y++)
            {
                inst = Instantiate(gridPf, content.transform);
                SetPositionInGrid(inst, new Vector2Int(x, y), Vector2Int.one);
            }
        }
        DrawItems();
    }

    private void SetPositionInGrid(GameObject obj, Vector2Int position, Vector2Int size)
    {
        var rectTrans = obj.transform as RectTransform;
        var cellPosition = GetGridPositionFromCell(position);
        rectTrans.sizeDelta = new Vector2(cellSize.x * size.x, cellSize.y * size.y);
        rectTrans.localPosition = new Vector3(cellPosition.x, -cellPosition.y, 0f);
    }
    private void DrawItems()
    {
        itemObjects = new GameObject[inventory.Size.x, inventory.Size.y];
        for (int i = 0; i < inventory.cells.Count; i++)
        {
            itemObjects[inventory.cells[i].position.x, inventory.cells[i].position.y] = DrawItem(inventory.cells[i]);
        }
    }
    private GameObject DrawItem(Cell cell)
    {
        var inst = Instantiate(itemPF, content.transform);
        var image = inst.GetComponent<Image>();
        image.sprite = cell.item.Sprite;
        var pos = new Vector2Int(cell.position.x, cell.position.y);
        SetPositionInGrid(inst, pos, cell.item.Size);
        cell.inventoryUI = this;
        inst.GetComponent<DragDrop>().disabled = cell.Disabled;
        return inst;
    }
    private Vector2 GetGridPositionFromCell(Vector2Int cellPosition)
    {
        return new Vector2(cellPosition.x * cellSize.x, cellPosition.y * cellSize.y);
    }
    private Vector2Int GetCellPositionFromGrid(Vector2 position)
    {
        var posX = Mathf.FloorToInt(position.x / cellSize.x);
        var posY = Mathf.FloorToInt(-position.y / cellSize.y);
        return new Vector2Int(posX, posY);
    }
    public void MoveItem(Vector2 originPos, Vector2 position)
    {
        inventory.MoveItem(GetCellPositionFromGrid(originPos), GetCellPositionFromGrid(position));
    }
    public void MoveItemToOtherInventory(InventoryUI otherInvUI, Vector2 targetPosition, Vector2 originPosition)
    {
        var cellPos = GetCellPositionFromGrid(originPosition);
        var targetCellPos = otherInvUI.GetCellPositionFromGrid(targetPosition);
        if (inventory.TryGetCellWithPosition(cellPos, out Cell cell))
        {
            if (otherInvUI.inventory.IsPossibleToPlaceItem(targetCellPos, cell, out List<Cell> wi))
            {
                cell.position = targetCellPos;
                otherInvUI.inventory.AddItem(cell);
                inventory.RemoveItem(cell);
            }
            else
            {
                DrawCellGrid();
            }
        }
        else
        {
            DrawCellGrid();
        }
    }
    public void SetItemColor(Cell cell, Color color)
    {
        itemObjects[cell.position.x, cell.position.y].GetComponent<Image>().color = color;
    }
    public void DisableItemDrag(Cell cell, bool value)
    {
        var dd = itemObjects[cell.position.x, cell.position.y].GetComponent<DragDrop>();
        dd.disabled = value;
    }
}
