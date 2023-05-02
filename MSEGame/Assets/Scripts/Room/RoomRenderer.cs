using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject treasure;

    public void Render(DoorCard card)
    {
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/room_default.png") as Sprite;
        door.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/door_closed.png") as Sprite;
        enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/monster_pig.png") as Sprite;

        if (card is MonsterCard)
            treasure.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/crate.png") as Sprite;
    }
}
