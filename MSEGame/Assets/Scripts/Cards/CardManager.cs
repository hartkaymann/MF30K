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
    public void InstantiateCard(Card card)
    {
        Vector3 position = Vector2.zero;
        if (card is DoorCard)
            position = doorStackTransform.position;
        else if (card is TreasureCard)
            position = treasureStackTransform.position;

        GameObject go = Instantiate(cardPrefab, position, Quaternion.identity);
        go.transform.SetParent(canvas, false);

        if (go.TryGetComponent<CardController>(out var cardController))
        {
            cardController.Card = card;

            StartCoroutine(AnimationManager.Instance.MoveFromTo(
                cardController.gameObject.transform,
                position,
                new Vector3(Screen.width / 2f, Screen.height / 2f, 0f),
                1.0f));
        }
    }
}
