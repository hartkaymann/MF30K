using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    public Image image;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);

        // Resize dragged object to fit inside grid
        GridLayoutGroup grid = parentAfterDrag.GetComponent<GridLayoutGroup>();
        if(grid != null)
        {
            float cellHeight = grid.cellSize.y;
            RectTransform rt = image.GetComponent<RectTransform>();
            float scaleFactor =  Mathf.Min(cellHeight / rt.rect.height, 1f);

            Debug.Log("Scale: " + scaleFactor);
            transform.localScale = Vector2.one * scaleFactor;
        }

        image.raycastTarget = true;
    }

}
