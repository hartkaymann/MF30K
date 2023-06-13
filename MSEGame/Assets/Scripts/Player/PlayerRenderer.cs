using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerRenderer : MonoBehaviour
{
    public void Render(Player player)
    {
        GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite(player.Race.ToString());
    }
}
