using UnityEngine;

public abstract class TreasureCard : Card
{

    public int value;
    public int bonus;

    public TreasureCard(string name, CardType type, string id, Sprite artwork, int value, int bonus) : base(name, type, id, artwork)
    {
        this.value = value;
        this.bonus = bonus;
    }
}
