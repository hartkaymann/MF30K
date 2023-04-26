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

    public Card(string title, CardType type, Sprite artwork)
    {
        this.title = title;
        this.type = type;
        this.artwork = null;
    }
}
