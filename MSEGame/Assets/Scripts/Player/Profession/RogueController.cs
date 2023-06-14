public class RogueController : ProfessionController
{
    public bool IsActive { get; private set; }

    protected override void Init()
    {
        AbilityName = "Death's Gambit";
        Cooldown = 3;
        Description = "A double edged sword!\nGet double tresures when you win.\nBut lose the chance to escpace when defeated.";
    }

    public override void UseAbility()
    {
        base.UseAbility();
        IsActive = true;
        if (RoomManager.Instance.CurrentRoom.Card is MonsterCard monster)
            monster.Treasures += 1; IsActive = true;
    }

    public override void HandleNewCycle()
    {
        base.HandleNewCycle();
        IsActive = false;
    }
}
