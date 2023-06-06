using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : Manager<AnimationManager>
{
    [Serializable]
    public struct NamedAnimCtrl
    {
        public string name;
        public AnimatorController animator;
    }

    [SerializeField] private NamedAnimCtrl[] namedAnimators;
    private Dictionary<string, AnimatorController> animators;

    protected override void Init()
    {
        animators = new Dictionary<string, AnimatorController>();
        for (int i = 0; i < namedAnimators.Length; i++)
        {
            NamedAnimCtrl ns = namedAnimators[i];
            animators.Add(ns.name, ns.animator);
        }
    }

    public AnimatorController GetAnimator(string name)
    {
        return animators[name];
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
        transform.Rotate(Vector2.up, 180f);
        yield return MoveFromTo(transform, targetPos, start, duration / 2f);
        transform.Rotate(Vector2.up, -180f);
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
