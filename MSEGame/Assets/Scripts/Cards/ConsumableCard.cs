using UnityEngine;

public class ConsumableCard : TreasureCard
{
    public int value;
    public int bonus;

    public ConsumableCard(string name, Sprite artwork, int value, int bonus) : base(name, CardType.Consumable, artwork){
        this.value = value;
        this.bonus = bonus;
    }
}
