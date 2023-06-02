using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    protected Image raycastImage;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        raycastImage.raycastTarget = false;

        // Increase shadow
        if (transform.Find("Bounds").TryGetComponent<Shadow>(out var shadow))
        {
            shadow.effectDistance = new(6, -10);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleEndDrag();

        // Increase shadow
        if (transform.Find("Bounds").TryGetComponent<Shadow>(out var shadow))
        {
            shadow.effectDistance = new(3, -5);
        }
    }

    protected virtual void HandleEndDrag()
    {
        transform.SetParent(parentAfterDrag);

        raycastImage.raycastTarget = true;
    }
}
