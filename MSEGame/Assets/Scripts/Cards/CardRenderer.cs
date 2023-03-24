using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
    public Card card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    public Image artworkImage;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI statText;

    void Start()
    {
       
    }

    public void setCard(Card card)
    {
        this.card = card;
        UpdateCardData();
    }

    void UpdateCardData()
    {
        nameText.text = card.name;
        typeText.text = card.type;

        artworkImage.sprite = card.artwork;

        costText.text = card.cost.ToString();
        statText.text = card.cost.ToString();
    }

}
