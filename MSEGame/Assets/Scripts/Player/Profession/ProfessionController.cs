using UnityEngine;
using UnityEngine.UI;

public abstract class ProfessionController : MonoBehaviour
{
    public string AbilityName { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CooldownRemaining { get; protected set; }

    private void Start()
    {
        GameManager.OnNewCycle += HandleNewCycle;
    }

    public virtual void UseAbility()
    {
        CooldownRemaining = Cooldown;
    }

    public virtual void HandleNewCycle()
    {
        if(CooldownRemaining > 0)
            CooldownRemaining--;
    }
}
