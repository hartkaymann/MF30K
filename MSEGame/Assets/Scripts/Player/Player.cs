using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public class Player
{
    [JsonProperty] private string name;
    [JsonProperty] private int level;
    [JsonProperty] private int combatLevel;

    [JsonProperty] private Gender gender;
    [JsonProperty] private Race race;
    [JsonProperty] private Profession profession;

    [JsonProperty] public int gold;
    [JsonIgnore] private int roundBonus;
    [JsonIgnore] private int raceEffect;

    [JsonProperty] private Dictionary<EquipmentSlot, EquipmentCard> equipment;

    public event Action OnPropertyChanged;
    public event Action OnProfessionChanged;
    public event Action OnRaceChanged;

    [JsonIgnore]
    public string Name
    {
        get => name;
        set
        {
            if (value != name)
            {
                name = value;
                OnPropertyChanged?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public int Level
    {
        get => level;
        set
        {
            if (value != level)
            {
                level = value;
                OnPropertyChanged?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public int CombatLevel
    {
        get => combatLevel;
        set
        {
            if (value != combatLevel)
            {
                combatLevel = value;
                OnPropertyChanged?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public Gender Gender
    {
        get => gender;
        set
        {
            if (value != gender)
            {
                gender = value;
                OnPropertyChanged?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public Race Race
    {
        get => race;
        set
        {
            race = value;
            Debug.Log("Invoking On race Changed");
            OnRaceChanged?.Invoke();
        }
    }

    [JsonIgnore]
    public Profession Profession
    {
        get => profession;
        set
        {
            profession = value;
            Debug.Log("Invoking On Profession Changed");
            OnProfessionChanged?.Invoke();
        }
    }

    [JsonIgnore]
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            //TODO: Set level up amount somewhere else, also when your item is worth more than 10 gold this fails
            if (gold >= 10)
            {
                gold %= 10;
                Level += 1;
            }
            OnPropertyChanged?.Invoke();
        }
    }

    [JsonIgnore]
    public int RoundBonus
    {
        get
        {
            return roundBonus;
        }
        set
        {
            if (roundBonus != value)
            {
                roundBonus = value;
                CalculateCombatLevel();
                OnPropertyChanged?.Invoke();
            }
        }
    }

    [JsonIgnore]
    public int RaceEffect
    {
        get
        {
            return raceEffect;
        }
        set
        {
            if (raceEffect != value)
            {
                raceEffect = value;
                CalculateCombatLevel();
                OnPropertyChanged?.Invoke();
            }
        }
    }

    public Dictionary<EquipmentSlot, EquipmentCard> Equipment
    {
        get
        {
            return equipment;
        }
        private set
        {
            if (equipment != value)
            {
                equipment = value;
                CalculateCombatLevel();
                Debug.Log("Equipment setter called private");
            }
        }
    }

    public Player(string name, Race race, Profession profession, Gender gender, int level, int combatLevel)
    {
        Debug.Log("Creating new player instance: " + name.ToString());
        this.name = name;
        this.race = race;
        this.profession = profession;
        this.gender = gender;
        this.level = level;
        this.combatLevel = combatLevel;

        gold = 0;
        roundBonus = 0;

        Debug.Log("Creating player inventory");
        equipment = new Dictionary<EquipmentSlot, EquipmentCard>();
    }

    public void CalculateCombatLevel()
    {
        int newCombatLevel = 0;
        foreach (var card in equipment.Values)
        {
            if (card == null)
                continue;

            newCombatLevel += card.bonus;
        }
        CombatLevel = newCombatLevel + roundBonus + raceEffect;
        Debug.Log($"Combat level: {CombatLevel} (Base: {newCombatLevel}, Bonus: {roundBonus}, Passive: {raceEffect})");
    }

    public static Player GetDummy()
    {
        return new("Kay", Race.Elf, Profession.Wizard, Gender.Male, 1, 0);
    }

}
