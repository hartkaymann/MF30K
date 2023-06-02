public class KnightController : ProfessionController
{

    private void Start()
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
