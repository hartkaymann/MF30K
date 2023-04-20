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

    public ProfessionCard(Profession profession, Sprite artwork, int cost, int stat) : base(profession.ToString(), CardType.Profession, artwork, cost, stat)
    {
        this.profession = profession;
    }
}
