using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsequenceCardController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject frontFace;
    [SerializeField] private GameObject backFace;

    public int consequence;

    // Flipping card
    private bool isFlipped = false;

    public void Start()
    {
        frontFace.SetActive(false);
        backFace.SetActive(true);
    }

    public virtual void Flip()
    {
        frontFace.SetActive(true);
        backFace.SetActive(false);



        GameManager.Instance.ConsequenceChosen(consequence);
    }

    private void Reset()
    {
        frontFace.SetActive(false);
        backFace.SetActive(false);
        isFlipped = false;
    }

    public IEnumerator AnimateFlip()
    {
        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, Vector3.zero, new Vector3(0f, 90f, 0f), 0.5f);

        Flip();

        yield return AnimationManager.Instance.RotateFromTo(gameObject.transform, new Vector3(0f, 90f, 0f), new Vector3(0f, 0f, 0f), 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if no siblings have already been flipped
        bool otherIsFlipped = false;
        foreach (var ccc in transform.parent.GetComponentsInChildren<ConsequenceCardController>())
        {
            if (ccc.isFlipped) otherIsFlipped = true;
        }

        if (!otherIsFlipped && !isFlipped)
            StartCoroutine(AnimateFlip());

        isFlipped = true;
    }

    public void OnEnable()
    {
        Reset();
    }
}
