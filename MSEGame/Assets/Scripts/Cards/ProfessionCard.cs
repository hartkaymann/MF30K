using UnityEngine;

public enum Profession
{
    Wizard,
    Rogue,
    Barbarian,
    Cleric
}

public class ProfessionCard : DoorCard
{
    private Profession profession;
    private string ability;

    public ProfessionCard(Profession profession, string id, Sprite artwork) : base(profession.ToString(), CardType.Profession, id, artwork)
    {
        this.profession = profession;
    }
}
