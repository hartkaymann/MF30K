using UnityEngine;

public class RoomController : MonoBehaviour
{
    private DoorCard card;

    public NpcController NPC { get; set; }

    // Door controlls
    private Animator doorAnimator;
    private int isDoorOpenHash;

    private Transform playerPosition;
    private RoomRenderer roomRenderer;
    public RoomRenderer Renderer { get { return roomRenderer; } }

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
                Debug.Log("Renderer: " + roomRenderer.name);
                Debug.Log("Card: " + card.title);
                roomRenderer.Render(card);
            }
        }
    }

    private void Awake()
    {
        doorAnimator = transform.Find("Door").GetComponent<Animator>();
        isDoorOpenHash = Animator.StringToHash("isOpen");

        roomRenderer = gameObject.GetComponent<RoomRenderer>();
        playerPosition = transform.Find("Player").transform;
    }

    private void Start()
    {
        PlayerController pc = PlayerManager.Instance.PlayerController;
        if (pc != null)
            pc.gameObject.transform.position = playerPosition.position;
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        doorAnimator.SetBool(isDoorOpenHash, true);
    }

    [ContextMenu("Close Door")]
    public void CloseDoor()
    {
        doorAnimator.SetBool(isDoorOpenHash, false);
    }
}
