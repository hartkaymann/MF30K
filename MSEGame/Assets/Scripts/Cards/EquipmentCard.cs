using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Helmet,
    Armor,
    Boots
}

public class EquipmentCard : Card
{

    private EquipmentType equipType;
    public EquipmentCard(string name, EquipmentType equipType, Sprite artwork, int cost, int stat) : base(name, CardTypes.Equipment, artwork, cost, stat)
    {
        this.equipType = equipType;
    }
}
