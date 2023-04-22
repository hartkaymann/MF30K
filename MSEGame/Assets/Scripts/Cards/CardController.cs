using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private Card card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    public Image artworkImage;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI statText;

    public void Render()
    {
        nameText.text = card.title;
        typeText.text = card.type.ToString().Substring(0, 1);

        artworkImage.sprite = card.artwork;

        costText.text = card.cost.ToString();
        statText.text = card.cost.ToString();
    }

    public void setCard(Card card)
    {
        this.card = card;
    }

    public Card getCard()
    {
        return card;
    }

}
