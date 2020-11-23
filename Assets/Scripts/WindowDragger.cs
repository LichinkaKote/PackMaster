using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragger : MonoBehaviour, IDragHandler
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private RectTransform target = null;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (target == null)
        {
            if (canvas == null)
            {
                GetComponent<RectTransform>().anchoredPosition += eventData.delta;
            }
            else
            {
                GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
        else
        {
            if (canvas == null)
            {
                target.anchoredPosition += eventData.delta;
            }
            else
            {
                target.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
        
        
    }

}
