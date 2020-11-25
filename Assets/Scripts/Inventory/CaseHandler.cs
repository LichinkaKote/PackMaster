using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseHandler : MonoBehaviour
{
    private Vector2 invUiOffset = new Vector2(50, -50);
    [SerializeField] private RectTransform body, workspace;
    private SuitCase suitCase;
    private InventoryUI invUI;
    private RectTransform bodyInst;
    private Animator animator;
    private Vector3 pos;
    public InventoryUI InvUI { get => invUI; }
    public RectTransform Rect { get => bodyInst; }

    public static CaseHandler Create(SuitCase suitCase)
    {
        var inst = Instantiate(Prefabs.casePf);
        var caseH = inst.GetComponent<CaseHandler>();
        caseH.suitCase = suitCase;
        caseH.DrawCase();
        return caseH;
    }

    private void DrawCase()
    {
        bodyInst = Instantiate(body, workspace);
        animator = bodyInst.GetComponent<Animator>();
        invUI = InventoryUI.Create(suitCase.CellGridSize.x, suitCase.CellGridSize.y);
        invUI.transform.SetParent(bodyInst);
        SetupGridParams(invUI);
        FitBodyToGrid(bodyInst);
    }
    
    private void SetupGridParams(InventoryUI invUI)
    {
        var invUiRect = invUI.transform as RectTransform;
        invUiRect.sizeDelta = Vector2.zero;
        invUiRect.anchorMin = Vector2.zero;
        invUiRect.anchorMax = Vector2.one;
        invUiRect.anchoredPosition = Vector2.zero;
        invUI.SetPosition(invUiOffset);
        invUI.SetCellSize(suitCase.CellSize);
        var inventory = new Inventory();
        inventory.Size = suitCase.CellGridSize;
        invUI.SetInventory(inventory);
    }
    private void FitBodyToGrid(RectTransform bodyRect)
    {
        float sizeX = suitCase.CellGridSize.x * suitCase.CellSize.x + invUiOffset.x * 2f;
        float sizeY = suitCase.CellGridSize.y * suitCase.CellSize.y + -invUiOffset.y * 2f;
        bodyRect.sizeDelta = new Vector2(sizeX, sizeY);
    }
    public IEnumerator CloseAnim(string trigger)
    {
        animator.SetTrigger(trigger);
        yield return new WaitForSeconds(1.2f);
        
        workspace.anchoredPosition = new Vector3(5000, 0, 0);
        animator.SetTrigger("Idle");
    }
    public void RestoreBody()
    {
        //gameObject.SetActive(true);
        workspace.anchoredPosition = new Vector3(0, 0, 0);
    }
}
