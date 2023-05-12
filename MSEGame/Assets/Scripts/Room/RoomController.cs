using UnityEngine;

public class RoomController : MonoBehaviour
{

    private DoorCard card;
    [SerializeField] private RoomRenderer roomRenderer;
    public RoomRenderer Renderer { get { return roomRenderer; } }

    // Door controlls
    private Animator doorAnimator;
    private int isDoorOpenHash;

    private Transform playerPosition;

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

    private void Awake()
    {
        doorAnimator = transform.Find("Door").GetComponent<Animator>();
        isDoorOpenHash = Animator.StringToHash("isOpen");

        playerPosition = transform.Find("Player").transform;
    }

    private void Start()
    {
        PlayerManager.Instance.CurrentPlayer.gameObject.transform.position = playerPosition.position;
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
