using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : Draggable, IPointerDownHandler
{
    private Card card;

    private bool facedUp = false;

    [SerializeField] private CardRenderer cardRenderer;

    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;

    public Card Card
    {
        get
        {
            return card;
        }
        set
        {
            if (card != value)
            {
                card = value;
                cardRenderer.Render(card);
            }
        }
    }

    public void Start()
    {
        raycastImage = cardRenderer.Background;

        backFace.SetActive(true);
        frontFace.SetActive(false);
    }

    public IEnumerator Flip()
    {
        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, Vector3.zero, new Vector3(0f, 90f, 0f), 0.5f);

        frontFace.SetActive(!facedUp);
        backFace.SetActive(facedUp);

        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, new Vector3(0f, 90f, 0f), new Vector3(0f, 0f, 0f), 0.5f);

        facedUp = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!facedUp)
            StartCoroutine(Flip());
    }

}
