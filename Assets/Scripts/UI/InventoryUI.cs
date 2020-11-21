using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoObserver
{
    [SerializeField] private GameObject itemPF;
    [SerializeField] private RectTransform content;
    [SerializeField] private Button addItemBtn;
    private Inventory inventory = new Inventory();
    private Vector2 cellSize;
  
    private void Awake()
    {
        observable = inventory;
        addItemBtn.onClick.AddListener(() => inventory.AddItem(Prefabs.items[0]));
        CalculateCellSize();
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        CalculateCellSize();
        Resubscribe(inventory);
        //DrawItems();
        DrawCellGrid();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //DrawItems();
        DrawCellGrid();
    }
    public override void Notify()
    {
        //DrawItems();
        DrawCellGrid();
    }
    /*
    private void DrawItems()
    {
        ClearContent();
        for (int i = 0; i < inventory.items.Count; i++)
        {
            DrawItem(inventory.items[i]);
        }
    }
    private void DrawItem(Item item)
    {
        var inst = Instantiate(itemPF, content.transform);
        var image = inst.GetComponent<Image>();
        image.sprite = item.Sprite;
    }*/
    private void ClearContent()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    
    private void CalculateCellSize()
    {
        float contentW = content.rect.width;
        float contentH = content.rect.height;
        cellSize = new Vector2(contentW / inventory.Size.x, contentH / inventory.Size.y);
    }

    private void DrawCellGrid()
    {
        ClearContent();
        GameObject inst;
        for (int x = 0; x < inventory.Size.x; x++)
        {
            for (int y = 0; y < inventory.Size.y; y++)
            {
                inst = Instantiate(itemPF, content.transform);
                SetCellPosition(inst, new Vector2Int(x, y));
            }
        }
    }

    private void SetCellPosition(GameObject itemPf, Vector2Int position)
    {
        var rectTrans = itemPf.transform as RectTransform;
        var cellPosition = GetCellPosition(position);
        rectTrans.sizeDelta = new Vector2(cellSize.y, cellSize.x);
        rectTrans.localPosition = new Vector3(cellPosition.y, cellPosition.x, 0f);
    }

    private Vector2 GetCellPosition(Vector2Int cellPosition)
    {
        return new Vector2(cellPosition.x * -cellSize.x, cellPosition.y * cellSize.y);
    }
}
