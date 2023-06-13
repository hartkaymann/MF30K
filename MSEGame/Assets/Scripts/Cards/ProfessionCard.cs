using UnityEngine;

public enum Profession
{
    Knight,
    Wizard,
    Rogue
}

public class ProfessionCard : DoorCard
{
    public readonly Profession profession;
    public readonly string ability;

    public ProfessionCard(Profession profession, string id, Sprite artwork) : base(profession.ToString(), CardType.Profession, id, artwork)
    {
        this.profession = profession;
    }
}
