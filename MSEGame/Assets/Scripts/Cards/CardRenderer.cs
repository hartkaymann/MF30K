using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;

    public Image artworkImage;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI statText;

    public void Render(Card card)
    {
        Debug.Log("Rendering Card.");
        nameText.text = card.name;
        typeText.text = card.type.ToString().Substring(0, 1);

        artworkImage.sprite = card.artwork;

        costText.text = card.cost.ToString();
        statText.text = card.cost.ToString();
    }

}
