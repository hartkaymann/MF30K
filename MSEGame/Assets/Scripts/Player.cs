using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female
}

public class Player : MonoBehaviour
{
    public int id;
    public new string name;
    public int level = 0;
    public int combatLevel = 0;

    public Gender gender;
    public Race race;
    public Profession profession;

    public Player(string name, Gender gender, Race race, Profession profession)
    {
        this.id = 0;
        this.name = name;
        this.gender = gender;
        this.race = race;
        this.profession = profession;
    }
}
