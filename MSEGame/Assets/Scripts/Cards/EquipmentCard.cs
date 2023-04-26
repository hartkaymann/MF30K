using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Helmet,
    Armor,
    Boots
}

public class EquipmentCard : TreasureCard
{
    public  EquipmentType equipType;
    public int value;
    public int bonus;

    public EquipmentCard(string name, EquipmentType equipType, Sprite artwork, int value, int bonus) : base(name, CardType.Equipment, artwork)
    {
        this.equipType = equipType;
        this.value = value;
        this.bonus = bonus;
    }
}
