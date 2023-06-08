public class WizardController : ProfessionController
{

    protected override void Init()
    {
        AbilityName = "Charm Monster";
        Cooldown = 3;
        Description = "Charm the monster, halfing its attack power.";
    }

    public override void UseAbility()
    {
        base.UseAbility();

        if (RoomManager.Instance.CurrentRoom.Card is MonsterCard monster)
        {
            monster.CombatBuff = -(monster.Level / 2);
            RoomManager.Instance.CurrentRoom.NPC.UpdateInfo();
        }
    }
}
