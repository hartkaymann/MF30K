public class WizardController : ProfessionController
{

    private void Start()
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
