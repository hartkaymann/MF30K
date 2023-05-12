using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject treasure;

    public void Render(DoorCard card)
    {
        if (card is MonsterCard)
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureClosed");

        // Switch card type here later
        npc.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("Slime");

    }

    public void OpenTreasure(bool isEmpty)
    {
        if (isEmpty)
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureEmpty");
        else
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureOpen");
    }

    public void ToggleNpc()
    {
        npc.SetActive(!npc.activeInHierarchy);
    }
}
