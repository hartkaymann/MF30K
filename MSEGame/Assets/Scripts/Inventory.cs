using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IDropHandler
{
    [SerializeField] private int inventorySize;

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount < inventorySize)
        {
            GameObject dropped = eventData.pointerDrag;
            Draggable draggable = dropped.GetComponent<Draggable>();
            Attach(draggable);
        }
    }

    protected void Attach(Draggable draggable)
    {
        draggable.parentAfterDrag = transform;
    }
}
