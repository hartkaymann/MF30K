using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject treasure;

    public GameObject Background { get { return background; } }
    public GameObject Door { get { return door; } }
    public GameObject Enemy { get { return enemy; } }
    public GameObject Treasure { get { return treasure; } }

    public void Render(DoorCard card)
    {
        Debug.Log($"Rendering {card.type} room: {card.title}");
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/room_default.png") as Sprite;
        door.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/door_closed.png") as Sprite;
        enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load("Characters/Slime.png") as Sprite;

        if (card is MonsterCard)
            treasure.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/crate.png") as Sprite;
    }
}
