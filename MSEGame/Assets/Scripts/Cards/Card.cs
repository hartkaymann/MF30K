using UnityEngine;

public enum CardCategory
{
    Door,
    Treasure
}
public enum CardType
{
    Consumable,
    Equipment,
    Item,
    Profession,
    Race,
    Monster
}

public abstract class Card
{
    public string id;
    public string title;
    public CardType type;

    public Sprite artwork;

    public Card(string title, CardType type, string id, Sprite artwork)
    {
        this.title = title;
        this.type = type;
        this.id = id;
        this.artwork = null;
    }
}
