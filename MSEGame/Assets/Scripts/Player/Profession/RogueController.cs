public class RogueController : ProfessionController
{
    public bool IsActive { get; private set; }

    private void Start()
    {
        AbilityName = "Something Gambit";
        Cooldown = 3;
    }

    public override void UseAbility()
    {
        base.UseAbility();
        IsActive = true;
    }

    public override void HandleNewCycle()
    {
        base.HandleNewCycle();
        IsActive = false;
    }
}
