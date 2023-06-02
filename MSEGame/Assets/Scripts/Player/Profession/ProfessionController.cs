using UnityEngine;

public abstract class ProfessionController : MonoBehaviour
{
    public string AbilityName { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CooldownRemaining { get; protected set; }
    public string Description { get; protected set; }

    private void Awake()
    {
        GameManager.OnNewCycle += HandleNewCycle;
        Init();

        Debug.Log("Changeability called");
    }

    protected virtual void Init() { }

    private void Start()
    {
        UIManager.Instance.ChangeAbility(this);
    }

    public virtual void UseAbility()
    {
        CooldownRemaining = Cooldown;
        Debug.Log("Ability Cooldown: " + Cooldown);

    }

    public virtual void HandleNewCycle()
    {
        if (CooldownRemaining > 0)
            CooldownRemaining--;

        Debug.Log("Reducing cooldown: " + CooldownRemaining);
    }
}
