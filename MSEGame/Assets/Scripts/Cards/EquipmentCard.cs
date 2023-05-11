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

    public EquipmentCard(string name, EquipmentType equipType, string id, Sprite artwork, int value, int bonus) : base(name, CardType.Equipment, id, artwork, value, bonus)
    {
        this.equipType = equipType;
    }
}
