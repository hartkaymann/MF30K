using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EquipmentType
{
    Weapon,
    Helmet,
    Armor,
    Boots
}

public class EquipmentCard : Card
{

    private EquipmentType type;
    public EquipmentCard(string name) : base(name)
    {

    }
}
