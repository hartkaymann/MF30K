using UnityEngine;

public class RoomController : MonoBehaviour
{

    [SerializeField] private RoomRenderer roomRenderer;
    public RoomRenderer Renderer { get { return roomRenderer; } }

    private DoorCard card;
    public DoorCard Card
    {
        get
        {
            return card;
        }
        set
        {
            if (card != value)
            {

                card = value;
                roomRenderer.Render(card);
            }

        }
    }
}
