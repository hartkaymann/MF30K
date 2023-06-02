using UnityEngine;

public class KnightController : ProfessionController
{
    public bool Active { get; set; }

    protected override void Init()
    {
        AbilityName = "Action Surge";
        Cooldown = 3;
        Description = "When defeated, get a secondchance to spin the wheel.";
    }

    public override void UseAbility()
    {
        base.UseAbility();

        Active = true;
    }
}
