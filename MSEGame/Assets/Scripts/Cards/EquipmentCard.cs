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
    public EquipmentCard(string name, EquipmentType equipType, Sprite artwork, int cost, int stat) : base(name, CardType.Equipment, artwork, cost, stat)
    {
        this.equipType = equipType;
    }
}
