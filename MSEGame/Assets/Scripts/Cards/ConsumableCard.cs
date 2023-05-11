using UnityEngine;

public enum BuffTarget
{
    Monster,
    Player,
    Both
}

public class ConsumableCard : TreasureCard
{
    public readonly BuffTarget target;

    public ConsumableCard(string name, string id, Sprite artwork, int value, int bonus, BuffTarget target) : base(name, CardType.Consumable, id, artwork, value, bonus){
        this.target = target;
    }
}
