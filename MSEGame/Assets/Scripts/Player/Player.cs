public enum Gender
{
    Male,
    Female
}

public class Player
{
    private string name;
    private int level;
    private int combatLevel;

    private Gender gender;
    private Race race;
    private Profession profession;

    public int gold;

    public string Name
    {
        get => name;
        set
        {
            if (value != name)
            {
                name = value;
                HandlePropertyChanged();
            }
        }
    }

    public int Level
    {
        get => level;
        set
        {
            if (value != level)
            {
                level = value;
                HandlePropertyChanged();
            }
        }
    }

    public int CombatLevel
    {
        get => combatLevel;
        set
        {
            if (value != combatLevel)
            {
                combatLevel = value;
                HandlePropertyChanged();
            }
        }
    }

    public Gender Gender
    {
        get => gender;
        set
        {
            if (value != gender)
            {
                gender = value;
                HandlePropertyChanged();
            }
        }
    }

    public Race Race
    {
        get => race;
        set
        {
            if (value != race)
            {
                race = value;
                HandlePropertyChanged();
            }
        }
    }

    public Profession Profession
    {
        get => profession;
        set
        {
            if (value != profession)
            {
                profession = value;
                HandlePropertyChanged();
            }
        }
    }

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
            HandlePropertyChanged();
        }
    }

    public Player(string name, Race race, Profession profession, Gender gender, int level, int combatLevel)
    {
        this.name = name;
        this.race = race;
        this.profession = profession;
        this.gender = gender;
        this.level = level;
        this.combatLevel = combatLevel;

        this.gold = 0;
    }

    private void HandlePropertyChanged()
    {
        PlayerManager.Instance.UpdatePlayerInfo(this);
        NetworkManager.Instance.PutPlayer(this);
    }

    public static Player GetDummy()
    {
        return new("Kay", Race.Human, Profession.Barbarian, Gender.Male, 1, 0);
    }

}
