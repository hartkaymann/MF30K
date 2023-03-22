using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{

    private string name;

    public Sprite image;
    public Sprite border;

    public Card(string name)
    {
        this.name = name;
    }
}
