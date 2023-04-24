using UnityEngine;

public enum Profession
{
    Warrior,
    Mage,
    Priest,
    Ranger
}

public class ProfessionCard : DoorCard
{
    private Profession profession;
    private string ability;

    public ProfessionCard(Profession profession, Sprite artwork) : base(profession.ToString(), CardType.Profession, artwork)
    {
        this.profession = profession;
    }
}
