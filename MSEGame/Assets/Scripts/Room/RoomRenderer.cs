using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject treasure;

    public void Render(Room room)
    {
        background.GetComponent<SpriteRenderer>().sprite = room.sprite;
    }
}
