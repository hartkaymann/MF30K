using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance { get; private set; }


    [Serializable]
    public struct NamedSprite
    {
        public string name;
        public Sprite sprite;
    }
    [SerializeField] private NamedSprite[] namedSprites;
    [SerializeField] private Sprite[] cardSprites;

    private Dictionary<string, Sprite> sprites;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        sprites = new Dictionary<string, Sprite>();
        for (int i = 0; i < namedSprites.Length; i++)
        {
            NamedSprite ns = namedSprites[i];
            sprites.Add(ns.name, ns.sprite);
        }
    }

    private void Start()
    {
    }

    public Sprite GetSprite(string name)
    {
        return sprites[name];
    }

    public Sprite GetCardSprite()
    {
        int idx = Random.Range(0, cardSprites.Length);
        return cardSprites[idx];
    }

    public Sprite GetDummySprite()
    {
        return Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.one * 0.5f);
    }

}
