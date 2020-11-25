using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Image image;
    private InventoryUI selfInventoryUI;
    private Vector2 originPosition;
    public bool disabled = false;
    private void Awake()
    {
        rectTransform = transform as RectTransform;
        image = GetComponent<Image>();
    }
    private void Start()
    {
        selfInventoryUI = transform.GetComponentInParent<InventoryUI>();
        canvas = transform.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!disabled)
        {
            image.raycastTarget = false;
            originPosition = rectTransform.anchoredPosition;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!disabled)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!disabled)
        {
            canvas.sortingOrder = 0;
            canvas.overrideSorting = false;
            Vector2 dropPos = rectTransform.anchoredPosition;
            var inventoryUI = eventData.pointerCurrentRaycast.gameObject?.transform.GetComponentInParent<InventoryUI>();
            if (inventoryUI != null)
            {
                dropPos += GetOffsetToDropPosition(selfInventoryUI.CellSize);
                if (inventoryUI == selfInventoryUI)
                {
                    inventoryUI.MoveItem(originPosition, dropPos);
                }
                else
                {
                    Vector3 objectWorldPos = eventData.pointerDrag.transform.position;
                    Vector3 targetGridLocalPos = eventData.pointerCurrentRaycast.gameObject.transform.parent.InverseTransformPoint(objectWorldPos);
                    targetGridLocalPos += (Vector3)GetOffsetToDropPosition(selfInventoryUI.CellSize);
                    selfInventoryUI.MoveItemToOtherInventory(inventoryUI, targetGridLocalPos, originPosition);
                }
            }
            else
            {
                rectTransform.anchoredPosition = originPosition;
            }
            image.raycastTarget = true;
        }
    }
    private Vector2 GetOffsetToDropPosition(Vector2 cellSize)
    {
        return new Vector2(cellSize.x * 0.75f, -cellSize.y * 0.75f);
    }
}

