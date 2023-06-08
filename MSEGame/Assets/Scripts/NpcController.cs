using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NpcController : MonoBehaviour
{
    [SerializeField]
    GameObject infoPrefab;
    private GameObject info;

    private Animator animator;
    private int deathHash;

    [SerializeField] private AudioSource soundIdle;
    [SerializeField] private AudioSource soundDeath;

    private void Awake()
    {
        if (info != null)
        {

            info = Instantiate(infoPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("UI").transform);
            if (info.TryGetComponent<ObjectFollow>(out var follow))
            {
                follow.Follow = transform.Find("Info");
            }
            UpdateInfo();
        }

        animator = GetComponent<Animator>();
        deathHash = Animator.StringToHash("Death");
    }

    public void UpdateInfo()
    {
        if (info.transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out var infoName))
        {
            infoName.text = RoomManager.Instance.CurrentRoom.Card.title;
        }

        Transform level = info.transform.Find("Level");
        if (level != null && level.TryGetComponent<TextMeshProUGUI>(out var infoLevel))
        {
            if (RoomManager.Instance.CurrentRoom.Card is MonsterCard card)
            {
                infoLevel.text = card.Level.ToString();

                if (card.CombatBuff > 0)
                    infoLevel.color = GameColor.Green;
                else if (card.CombatBuff < 0)
                    infoLevel.color = GameColor.Red;
                else
                    infoLevel.color = GameColor.White;
            }
        }
    }

    [ContextMenu("Commit Die")]
    public void Die()
    {
        if (soundIdle != null)
            soundIdle.Stop();
        if (soundDeath != null)
            soundDeath.Play();

        if (animator != null)
        {
            animator.SetTrigger(deathHash);
        }
    }
}
