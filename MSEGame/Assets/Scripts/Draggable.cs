using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    protected Image raycastImage;


    void Update()
    {
        //transform.localScale = Vector3.one * ( 1f + 0.5f * Mathf.Sin(Time.time));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        raycastImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleEndDrag();
    }

    protected virtual void HandleEndDrag()
    {
        // Resize dragged object to fit inside grid
        float scaleFactor = 1.0f;
        if (parentAfterDrag.TryGetComponent<GridLayoutGroup>(out var grid))
        {
            float cellHeight = grid.cellSize.y;
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            scaleFactor = Mathf.Min(cellHeight / rt.rect.height, 1f);
        }

        transform.localScale = Vector3.one * scaleFactor;

        transform.SetParent(parentAfterDrag);

        raycastImage.raycastTarget = true;
    }
}
