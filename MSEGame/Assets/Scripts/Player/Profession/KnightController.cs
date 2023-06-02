public class KnightController : ProfessionController
{

    protected override void Init()
    {
        AbilityName = "Something Something Valor";
        Cooldown = 3;
    }

    public override void UseAbility()
    {
        base.UseAbility();

        // do a thing
    }
}
