using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{

    private bool isEmpty = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (isEmpty)
        {
            isEmpty = true;
            GameObject dropped = eventData.pointerDrag;
            Draggable draggable = dropped.GetComponent<Draggable>();
            draggable.parentAfterDrag = transform;
        }
    }
}
