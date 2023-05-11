using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform canvas;

    [SerializeField] private Transform doorStackTransform;
    [SerializeField] private Transform treasureStackTransform;

    void Awake()
    {
        instance = this;
    }
    public CardController InstantiateCard(Card card)
    {
        GameObject go = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
        go.transform.SetParent(canvas, false);

        if (go.TryGetComponent<CardController>(out var cardController))
        {
            cardController.Card = card;
        }

        return cardController;
    }

    public void DrawCardFromStack(Card card)
    {
        CardController controller = InstantiateCard(card);

        // Flip card  so back is up
        controller.Flip();

        // Move to appropriate deck position
        Vector3 position = Vector2.zero;
        if (card is DoorCard)
            position = doorStackTransform.position;
        else if (card is TreasureCard)
            position = treasureStackTransform.position;

        // Start animaton
        StartCoroutine(AnimationManager.Instance.MoveFromTo(
                controller.gameObject.transform,
                position,
                new Vector3(Screen.width / 2f, Screen.height / 2f, 0f),
                1.0f));
    }
}
