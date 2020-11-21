using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefabs
{
    public static Item[] items;

    public static GameObject inventoryUI;

    private static bool loaded = LoadResources();
    private static bool LoadResources()
    {
        items = Resources.LoadAll<Item>("ScriptObjects/Items");
        inventoryUI = Resources.Load<GameObject>("Prefabs/UI/InventoryUI");
        return true;
    }
}
