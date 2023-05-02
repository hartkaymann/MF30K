using UnityEngine;

public class RoomController : MonoBehaviour
{

    [SerializeField] private RoomRenderer roomRenderer;
    [SerializeField] public RoomRenderer Renderer { get { return roomRenderer; } }

    // Current room infomation
    private DoorCard card;
    public DoorCard Card
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
            roomRenderer.Render(value);
        }
    }

    void Start()
    {
    }

}
