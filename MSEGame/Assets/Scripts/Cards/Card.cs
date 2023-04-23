using UnityEngine;

public enum CardType
{
    Door,
    Treasure,
    Consumable,
    Equipment,
    Item,
    Profession,
    Race,
    Monster
}

public abstract class Card
{
    public int id;
    public string title;
    public CardType type;

    public Sprite artwork;

    public Card(string name, CardType type, Sprite artwork)
    {
        this.title = name;
        this.type = type;
        this.artwork = null;
    }
}
