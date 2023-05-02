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

    public Player(string name)
    {
        Name = name;
    }

    public void HandlePropertyChanged()
    {
        NetworkManager.instance.PutPlayer(this);
    }


}
