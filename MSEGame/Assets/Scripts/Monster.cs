using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    private int combatLevel;
    
    public Monster(int combatLevel)
    {
        this.combatLevel = combatLevel;
    }

    public int GetCombatLevel()
    {
        return combatLevel;
    }
}
