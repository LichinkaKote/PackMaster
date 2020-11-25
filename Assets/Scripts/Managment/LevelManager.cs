using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private Level level;
    private SuitCase suitCase;
    private CaseHandler[,] createdCase;
    private Country country;
    private List<Cell> addedItems, existedItems;
    private ItemStore itemStore;
    private InventoryUI bottomPanel;
    private List<Item> levelPosibleItems;
    private Vector2 offset = new Vector2(.9f, .9f);
    public event System.Action<bool> retryLevel;
    public event System.Action updateLevelUI;
    public event System.Action blockStartBtn;
    private Inventory MainInventory { get => createdCase[createdCase.GetLength(0) - 1, createdCase.GetLength(1) - 1].InvUI.Inventory; }
    public Country Country { get => country; }

    private void Awake()
    {
        SetLevel(GameManager.LevelToLoad);
    }
    public void SetLevel(Level level)
    {
        this.level = level;
        suitCase = level.PossibleSuitCases[Random.Range(0, level.PossibleSuitCases.Count)];
    }
    private void Start()
    {

        country = Prefabs.countries[Random.Range(0, Prefabs.countries.Length)];
        updateLevelUI?.Invoke();
        levelPosibleItems = GetLevelFitItems(country.PosibleItems);

        createdCase = CreateSuitCase(suitCase, new Vector2(0, 150));

        AddInitItems();
        SpreadItems(50);
        itemStore = new ItemStore();
        itemStore.SaveItems(existedItems);
        AddPlacebleItems();
        itemStore.SaveItems(addedItems);
    }
    
    private CaseHandler[,] CreateSuitCase(SuitCase suitCase, Vector2 position)
    {
        Vector2Int size = suitCase.Size;
        CaseHandler[,] caseHandlers = new CaseHandler[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var caseHandler = CaseHandler.Create(suitCase);
                caseHandler.Rect.anchoredPosition = position + new Vector2(x * offset.x * caseHandler.Rect.sizeDelta.x, y * offset.y * caseHandler.Rect.sizeDelta.y);
                caseHandlers[x, y] = caseHandler;

            }
        }
        return caseHandlers;
    }
    private List<Item> GetLevelFitItems(List<Item> items)
    {
        List<Item> sorted = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Size.x <= level.ItemSize.x && items[i].Size.y <= level.ItemSize.y)
            {
                sorted.Add(country.PosibleItems[i]);
            }
        }
        return sorted;
    }
    private void AddInitItems()
    {
        FillParams fillParams2 = new FillParams();
        fillParams2.posibleItems = levelPosibleItems;
        fillParams2.fillPercent = 50;
        existedItems = FillInventoryWithItems(MainInventory, fillParams2);
        foreach (var item in existedItems)
        {
            item.Disable(true);
        }
    }
    private void AddPlacebleItems()
    {
        var cellsize = new Vector2(125, 125);

        var invUi = InventoryUI.Create(8, 2);
        invUi.SetCellSize(cellsize);
        invUi.Rect.pivot = new Vector2(0.5f, 0);
        invUi.Rect.anchorMin = new Vector2(0.5f, 0);
        invUi.Rect.anchorMax = new Vector2(0.5f, 0);
        invUi.Rect.anchoredPosition = Vector2.zero;
        invUi.Rect.sizeDelta = invUi.ContentRect.sizeDelta;
        invUi.ContentRect.anchoredPosition = new Vector2(0, 230);
        FillParams fillParams = new FillParams();
        fillParams.posibleItems = levelPosibleItems;
        fillParams.fillPercent = 1;
        addedItems = FillInventoryWithItems(invUi.Inventory, fillParams);
        bottomPanel = invUi;
    }
    private void SpreadItems(float chance)
    {
        for (int x = createdCase.GetLength(0) - 1; x > 0; x--)
        {
            Inventory inventoryFrom = createdCase[x, createdCase.GetLength(1) - 1].InvUI.Inventory;
            Inventory inventoryTo = createdCase[x - 1, createdCase.GetLength(1) - 1].InvUI.Inventory;
            CloseCaseXAix(inventoryTo, inventoryFrom, chance);
        }

        for (int y = createdCase.GetLength(1) - 1; y > 0; y--)
        {
            for (int x = createdCase.GetLength(0) - 1; x >= 0; x--)
            {
                Inventory inventoryFrom = createdCase[x, y].InvUI.Inventory;
                Inventory inventoryTo = createdCase[x, y - 1].InvUI.Inventory;
                CloseCaseYAix(inventoryTo, inventoryFrom, chance);
            }
        }
    }
    private List<Cell> FillInventoryWithItems(Inventory inventory, FillParams fillParams)
    {
        List<Cell> addedItems = new List<Cell>();
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
            addedItems.Add(newCell);
        }
        return addedItems;
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
                if (inventory.IsPossibleToPlaceItem(position, testCell, out List<Cell> wi))
                {
                    posiblePositions.Add(position);
                }
            }
        }
        return posiblePositions;
    }
    public void LoadItems()
    {
        for (int i = 0; i < createdCase.GetLength(0); i++)
        {
            for (int j = 0; j < createdCase.GetLength(1); j++)
            {
                createdCase[i, j].InvUI.Inventory.Clear();
            }
        }
        bottomPanel.Inventory.Clear();
        for (int i = 0; i < itemStore.Cells.Count; i++)
        {
            var cell = itemStore.Cells[i];
            cell.position = itemStore.Positions[i];
            itemStore.Uis[i].Inventory.AddItem(cell);
        }
        RestoreCase();
        retryLevel?.Invoke(false);
    }
    private void RestoreCase()
    {
        for (int i = 0; i < createdCase.GetLength(0); i++)
        {
            for (int j = 0; j < createdCase.GetLength(1); j++)
            {
                createdCase[i, j].RestoreBody();
            }
        }
        
    }
    public void CloseCase()
    {
        if (CheckItems())
        {
            blockStartBtn?.Invoke();
            StartCoroutine(Close());
        }
        else
        {
            Debug.Log("Put items to case");
        }

    }
    private IEnumerator Close()
    {
        List<Cell> wrongItems = new List<Cell>();
        for (int x = 0; x < createdCase.GetLength(1) - 1; x++)
        {
            for (int y = 0; y < createdCase.GetLength(0); y++)
            {
                Inventory inventoryFrom = createdCase[y, x].InvUI.Inventory;
                Inventory inventoryTo = createdCase[y, x + 1].InvUI.Inventory;
                wrongItems.AddRange(CloseCaseYAix(inventoryTo, inventoryFrom));
                if (wrongItems.Count > 0)
                {
                    ColorWrongItems(wrongItems);
                    retryLevel?.Invoke(true);
                    StopAllCoroutines();
                }
                yield return createdCase[y, x].CloseAnim("CloseY");
            }
            
        }

        for (int x = 0; x < createdCase.GetLength(0) - 1; x++)
        {
            Inventory inventoryTo = createdCase[x + 1, createdCase.GetLength(1) - 1].InvUI.Inventory;
            Inventory inventoryFrom = createdCase[x, createdCase.GetLength(1) - 1].InvUI.Inventory;
            wrongItems.AddRange(CloseCaseXAix(inventoryTo, inventoryFrom));
            if (wrongItems.Count > 0)
            {
                ColorWrongItems(wrongItems);
                retryLevel?.Invoke(true);
                StopAllCoroutines();
            }
            yield return createdCase[x, createdCase.GetLength(1) - 1].CloseAnim("CloseX"); ;
        }
        AddMoney();
        yield return new WaitForSeconds(2);
        GameManager.LoadNextLevel();
    }
    private List<Cell> CloseCaseXAix(Inventory inventoryTo, Inventory inventoryFrom, float itemPercent = 100f)
    {
        List<Cell> imposibleItems = new List<Cell>();
        if (inventoryTo.Size == inventoryFrom.Size)
        {
            var size = inventoryTo.Size;
            for (int i = 0; i < inventoryFrom.cells.Count; i++)
            {
                if (itemPercent >= UnityEngine.Random.Range(0f, 100f))
                {
                    var positionInFrom = inventoryFrom.cells[i].position;
                    int posX = (size.x - 1) - positionInFrom.x - (inventoryFrom.cells[i].item.Size.x - 1);
                    var positionInTo = new Vector2Int(posX, positionInFrom.y);
                    if (inventoryTo.IsPossibleToPlaceItem(positionInTo, inventoryFrom.cells[i], out List<Cell> wrongItems))
                    {
                        inventoryFrom.cells[i].position = positionInTo;
                        inventoryTo.AddItem(inventoryFrom.cells[i]);
                        inventoryFrom.RemoveItem(inventoryFrom.cells[i]);
                        i--;
                    }
                    else
                    {
                        imposibleItems.AddRange(wrongItems);
                        return imposibleItems;
                    }
                }
            }
        }
        return imposibleItems;
    }
    private List<Cell> CloseCaseYAix(Inventory inventoryTo, Inventory inventoryFrom, float itemPercent = 100f)
    {
        List<Cell> imposibleItems = new List<Cell>();
        if (inventoryTo.Size == inventoryFrom.Size)
        {
            var size = inventoryTo.Size;
            for (int i = 0; i < inventoryFrom.cells.Count; i++)
            {
                if (itemPercent >= UnityEngine.Random.Range(0f, 100f))
                {
                    var positionInFrom = inventoryFrom.cells[i].position;
                    int posY = (size.y - 1) - positionInFrom.y - (inventoryFrom.cells[i].item.Size.y - 1);
                    var positionInTo = new Vector2Int(positionInFrom.x, posY);
                    if (inventoryTo.IsPossibleToPlaceItem(positionInTo, inventoryFrom.cells[i], out List<Cell> wrongItems))
                    {
                        inventoryFrom.cells[i].position = positionInTo;
                        inventoryTo.AddItem(inventoryFrom.cells[i]);
                        inventoryFrom.RemoveItem(inventoryFrom.cells[i]);
                        i--;
                    }
                    else
                    {
                        imposibleItems.AddRange(wrongItems);
                        return imposibleItems;
                    }
                }
            }
        }
        return imposibleItems;
    }
    private bool CheckItems()
    {
        int totalItems = addedItems.Count + existedItems.Count;

        int caseItems = 0;
        for (int i = 0; i < createdCase.GetLength(0); i++)
        {
            for (int j = 0; j < createdCase.GetLength(1); j++)
            {
                caseItems += createdCase[i, j].InvUI.Inventory.cells.Count;
            }
        }
        return totalItems == caseItems;
    }
    private void ColorWrongItems(List<Cell> wrongItems)
    {
        for (int i = 0; i < addedItems.Count; i++)
        {
            for (int j = 0; j < wrongItems.Count; j++)
            {
                if (addedItems[i] == wrongItems[j])
                {
                    wrongItems[j].SetColor(Color.red);
                    wrongItems.RemoveAt(j);
                    j--;
                }
            }
        }
    }
    private void AddMoney()
    {
        GameManager.money += Random.Range(10, 26);
        updateLevelUI?.Invoke();
    }
    private class ItemStore
    {
        private List<Cell> cells = new List<Cell>();
        private List<Vector2Int> positions = new List<Vector2Int>();
        private List<InventoryUI> uis = new List<InventoryUI>();

        public List<Cell> Cells { get => cells; }
        public List<Vector2Int> Positions { get => positions; }
        public List<InventoryUI> Uis { get => uis; }

        public void SaveItems(List<Cell> cell)
        {
            for (int i = 0; i < cell.Count; i++)
            {
                cells.Add(cell[i]);
                positions.Add(cell[i].position);
                uis.Add(cell[i].inventoryUI);
            }
        }
    }
}
