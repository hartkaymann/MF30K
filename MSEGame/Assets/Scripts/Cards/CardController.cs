using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : Draggable, IPointerDownHandler
{
    private Card card;
    [SerializeField] private CardRenderer cardRenderer;

    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;

    // Flipping card
    private bool facedUp = true;
    private bool isFlipped = false;

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

        facedUp = frontFace.activeInHierarchy;
        backFace.SetActive(!facedUp);
    }

    public virtual void Flip()
    {
        facedUp = !facedUp;

        frontFace.SetActive(facedUp);
        backFace.SetActive(!facedUp);
    }

    public IEnumerator AnimateFlip()
    {
        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, Vector3.zero, new Vector3(0f, 90f, 0f), 0.5f);

        Flip();  

        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, new Vector3(0f, 90f, 0f), new Vector3(0f, 0f, 0f), 0.5f);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!facedUp && !isFlipped)
            StartCoroutine(AnimateFlip());

        isFlipped = true;
    }

    public void Discard()
    {
        StartCoroutine(NetworkManager.Instance.DiscardCard(PlayerManager.Instance.CurrentPlayer.Player, Card));

        Destroy(gameObject);
    }
}
