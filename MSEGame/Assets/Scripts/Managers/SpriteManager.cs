using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteManager : Manager<SpriteManager>
{
    [Serializable]
    public struct NamedSprite
    {
        public string name;
        public Sprite sprite;
    }

    [SerializeField] private NamedSprite[] namedSprites;
    [SerializeField] private NamedSprite[] starterSprites;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private Sprite[] helmetSprites;
    [SerializeField] private Sprite[] armorSprites;
    [SerializeField] private Sprite[] bootSprites;
    [SerializeField] private Sprite[] consumablesSprites;
    [SerializeField] private Sprite[] raceSprites;
    [SerializeField] private Sprite[] professionSprites;

    private Dictionary<string, Sprite> sprites;
    private Dictionary<string, Sprite> starters;

    protected override void Init()
    {
        sprites = new Dictionary<string, Sprite>();
        for (int i = 0; i < namedSprites.Length; i++)
        {
            NamedSprite ns = namedSprites[i];
            sprites.Add(ns.name, ns.sprite);
        }

        starters = new Dictionary<string, Sprite>();
        for (int i = 0; i < starterSprites.Length; i++)
        {
            NamedSprite ns = starterSprites[i];
            starters.Add(ns.name, ns.sprite);
        }
    }

    public Sprite GetSprite(string name)
    {
        return sprites[name];
    }

    public Sprite GetEquipmentSprite(EquipmentType type)
    {
        Sprite sprite;
        switch (type)
        {
            case EquipmentType.Helmet:
                sprite = GetHelmetSprite();
                break;
            case EquipmentType.Armor:
                sprite = GetArmorSprite();
                break;
            case EquipmentType.Boots:
                sprite = GetBootSprite();
                break;
            case EquipmentType.Weapon:
                sprite = GetWeaponSprite();
                break;
            default:
                return null;
        }
        return sprite;
    }

    public Sprite GetStarterSprite(EquipmentSlot slot)
    {
        return starters[slot.ToString()];
    }

    public Sprite GetCardSprite()
    {
        int idx = Random.Range(0, cardSprites.Length);
        return cardSprites[idx];
    }

    public Sprite GetWeaponSprite()
    {
        int idx = Random.Range(0, weaponSprites.Length);
        return weaponSprites[idx];
    }

    public Sprite GetHelmetSprite()
    {
        int idx = Random.Range(0, helmetSprites.Length);
        return helmetSprites[idx];
    }

    public Sprite GetArmorSprite()
    {
        int idx = Random.Range(0, armorSprites.Length);
        return armorSprites[idx];
    }

    public Sprite GetBootSprite()
    {
        int idx = Random.Range(0, bootSprites.Length);
        return bootSprites[idx];
    }

    public Sprite GetConsumableSprite()
    {
        int idx = Random.Range(0, consumablesSprites.Length);
        return consumablesSprites[idx];
    }

    public Sprite GetRaceSprite()
    {
        int idx = Random.Range(0, raceSprites.Length);
        return raceSprites[idx];
    }

    public Sprite GetProfessionSprite()
    {
        int idx = Random.Range(0, professionSprites.Length);
        return professionSprites[idx];
    }

    public Sprite GetDummySprite()
    {
        return Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.one * 0.5f);
    }

}
