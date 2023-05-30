using UnityEngine;

public class DefeatPanelController : MonoBehaviour
{
    [SerializeField] private GameObject[] cards; 

    public void OnEnable()
    {
        DestroyCards();
        Shuffle(cards);
        CreateCards();
    }

    private void DestroyCards()
    {
        foreach (Transform child in transform.Find("Cards"))
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateCards()
    {
        foreach(GameObject card in cards)
        {
            Instantiate(card, transform.Find("Cards"));
        }
    }

    private static void Shuffle(GameObject[] items)
    {
        for (int i = 0; i < items.Length - 1; i++)
        {
            int j = Random.Range(i, items.Length);
            GameObject temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
