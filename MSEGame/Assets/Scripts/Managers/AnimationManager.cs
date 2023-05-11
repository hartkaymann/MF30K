using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public static AnimationManager Instance { get; private set; }

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
    }

    public IEnumerator MoveFromTo(Transform transform, Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }
    }

    public IEnumerator MoveAndBack(Transform transform, Vector3 targetPos, float duration)
    {
        Vector3 start = transform.position;

        yield return MoveFromTo(transform, start, targetPos, duration / 2f);
        yield return MoveFromTo(transform, targetPos, start, duration / 2f);
    }

    public IEnumerator RotateFromTo(Transform transform, Vector3 startAngle, Vector3 targetAngle, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startAngle), Quaternion.Euler(targetAngle), elapsedTime / duration);
            yield return null;
        }

    }
}
