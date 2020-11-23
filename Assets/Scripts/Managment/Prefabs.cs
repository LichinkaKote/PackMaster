using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefabs
{
    public static Item[] items;
    public static Level[] levels;

    public static GameObject inventoryUI;

    public static GameObject casePf;

    private static bool loaded = LoadResources();
    private static bool LoadResources()
    {
        items = Resources.LoadAll<Item>("ScriptObjects/Items");
        levels = Resources.LoadAll<Level>("ScriptObjects/Levels");
        inventoryUI = Resources.Load<GameObject>("Prefabs/UI/InventoryUI");
        casePf = Resources.Load<GameObject>("Prefabs/CasePf");
        return true;
    }
}
