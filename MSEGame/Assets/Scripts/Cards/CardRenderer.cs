using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{

    [SerializeField] private Image back;
    [SerializeField] private Image background;
    [SerializeField] private Image artwork;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private TextMeshProUGUI bonus;

    public Image Background { get { return background; } }

    public void Render(Card card)
    {

        back.sprite = SpriteManager.Instance.GetSprite((card is DoorCard) ? "DoorCard" : "TreasureCard");

        Sprite cardSprite = SpriteManager.Instance.GetCardSprite();
        if (cardSprite != null)
        {
            background.sprite = cardSprite;
        }

        type.text = card.type.ToString();
        title.text = card.title.ToString();

        if (card is TreasureCard treasureCard)
        {
            value.gameObject.SetActive(true);
            bonus.gameObject.SetActive(true);

            value.text = treasureCard.value.ToString();
            bonus.text = treasureCard.bonus.ToString();
        }
        else
        {
            value.gameObject.SetActive(false);
            bonus.gameObject.SetActive(false);

            value.text = "0";
            bonus.text = "0";
        }
    }
}
