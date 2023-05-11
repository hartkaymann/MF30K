using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject treasure;

    public GameObject Background { get { return background; } }
    public GameObject Door { get { return door; } }
    public GameObject Treasure { get { return treasure; } }
    public GameObject NPC { get { return npc; } }

    public void Render(DoorCard card)
    {
        background.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("Background");
        door.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("Door");

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
}
