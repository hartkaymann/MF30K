public class WizardController : ProfessionController
{

    protected override void Init()
    {
        AbilityName = "Enchant";
        Cooldown = 3;
    }
    public override void UseAbility()
    {
        base.UseAbility();

        if (RoomManager.Instance.CurrentRoom.Card is MonsterCard monster)
        {
            monster.level /= 2;
        }
    }
}
