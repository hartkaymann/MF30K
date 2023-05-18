using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject treasure;

    public void Render(DoorCard card)
    {
        if (card is MonsterCard)
        {
            npc.GetComponent<Animator>().enabled = true;
            npc.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("Slime");
            treasure.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetSprite("TreasureClosed");
        }
        else
        {
            npc.GetComponent<Animator>().enabled = false;
        }
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
