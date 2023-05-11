using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private Card card;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private Image artworkImage;

    public void Render()
    {
        nameText.text = card.title;
        typeText.text = card.type.ToString().Substring(0, 1);

        artworkImage.sprite = card.artwork;
    }

    public void setCard(Card card)
    {
        this.card = card;
        Render();
    }

    public Card getCard()
    {
        return card;
    }

}