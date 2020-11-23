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
        var cellsize = new Vector2(125, 125);

        var ui3 = InventoryUI.Create(8, 2);
        ui3.SetCellSize(cellsize);
        ui3.SetPosition(new Vector2(0, -1350));

        FillParams fillParams = new FillParams();
        fillParams.posibleItems = new List<Item>(Prefabs.items);
        fillParams.fillPercent = 50;
        FillInventoryWithItems(ui3.Inventory, fillParams);

        level = Prefabs.levels[0];
        var caseH = CaseHandler.Create(level.PossibleSuitCases[0]);
        caseH.Rect.anchoredPosition = new Vector2(0, 400);
        FillInventoryWithItems(caseH.InvUI.Inventory, fillParams);

        var offsetX = caseH.Rect.sizeDelta.x;
        var caseH2 = CaseHandler.Create(level.PossibleSuitCases[0]);
        caseH2.Rect.anchoredPosition = new Vector2(offsetX, 400);
    }

    private void CloseCaseVert(Inventory inventoryTo , Inventory inventoryFrom)
    {
        if (inventoryTo.Size == inventoryFrom.Size)
        {
            var size = inventoryTo.Size;
            for (int i = 0; i < inventoryFrom.cells.Count; i++)
            {
                var positionInFrom = inventoryFrom.cells[i].position;
                int posX = (size.x - 1) - positionInFrom.x - (inventoryFrom.cells[i].item.Size.x - 1);
                var positionInTo = new Vector2Int(posX, positionInFrom.y);
                if (inventoryTo.IsPossibleToPlaceItem(positionInTo, inventoryFrom.cells[i]))
                {
                    inventoryFrom.cells[i].position = positionInTo;
                    inventoryTo.AddItem(inventoryFrom.cells[i]);
                    inventoryFrom.RemoveItem(inventoryFrom.cells[i]);
                    i--;
                }
            }
        }
    }
    private void CloseCaseHoriz(Inventory inventoryTo, Inventory inventoryFrom)
    {
        if (inventoryTo.Size == inventoryFrom.Size)
        {
            var size = inventoryTo.Size;
            for (int i = 0; i < inventoryFrom.cells.Count; i++)
            {
                var positionInFrom = inventoryFrom.cells[i].position;
                int posY = (size.y - 1) - positionInFrom.y - (inventoryFrom.cells[i].item.Size.y - 1);
                var positionInTo = new Vector2Int(positionInFrom.x, posY);
                if (inventoryTo.IsPossibleToPlaceItem(positionInTo, inventoryFrom.cells[i]))
                {
                    inventoryFrom.cells[i].position = positionInTo;
                    inventoryTo.AddItem(inventoryFrom.cells[i]);
                    inventoryFrom.RemoveItem(inventoryFrom.cells[i]);
                    i--;
                }
            }
        }
    }


    private void FillInventoryWithItems(Inventory inventory, FillParams fillParams)
    {
        while (true)
        {
            int randonItemIndex = Random.Range(0, fillParams.posibleItems.Count);
            Item randomItem = fillParams.posibleItems[randonItemIndex];
            List<Vector2Int> posiblePositions = GetPosiblePositions(inventory, randomItem);
            if (posiblePositions.Count == 0) break;
            int totalCells = inventory.Size.x * inventory.Size.y;
            int ocuipedCells = totalCells - inventory.GetFreeCellsCount();
            if ((float)ocuipedCells / totalCells * 100f >= fillParams.fillPercent) break;
            var newCell = new Cell(randomItem, posiblePositions[Random.Range(0, posiblePositions.Count)]);
            inventory.AddItem(newCell);
        }
    }

    private List<Vector2Int> GetPosiblePositions(Inventory inventory, Item item)
    {
        int maxX = inventory.Size.x - item.Size.x;
        int maxY = inventory.Size.y - item.Size.y;
        List<Vector2Int> posiblePositions = new List<Vector2Int>();
        for (int x = 0; x <= maxX; x++)
        {
            for (int y = 0; y <= maxY; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                var testCell = new Cell(item, position);
                if (inventory.IsPossibleToPlaceItem(position, testCell))
                {
                    posiblePositions.Add(position);
                }
            }
        }
        return posiblePositions;
    }
}
