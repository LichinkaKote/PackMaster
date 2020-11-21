using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Level level;
    public void SetLevel(Level level)
    {
        this.level = level;
    }
    private void Start()
    {
        var inventory = new Inventory();
        inventory.Size = new Vector2Int(7, 7);
        inventory.AddItem(Prefabs.items[0]);
        var inst = Instantiate(Prefabs.inventoryUI);
        var invUI = inst.GetComponent<InventoryUI>();
        invUI.SetInventory(inventory);
    }

}
