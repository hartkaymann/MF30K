using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject treasure;

    public void Render(DoorCard card)
    {
        if (card is MonsterCard)
        {
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureClosed");
        }
        else
        {

        }
    }

    public void OpenTreasure(bool isEmpty)
    {
        if (isEmpty)
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureEmpty");
        else
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureOpen");
    }
}
