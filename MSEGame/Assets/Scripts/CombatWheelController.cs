using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatWheelController : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private Image playerSlice;
    [SerializeField] private Button buttonStart;

    [SerializeField] private float spinTime = 5f;
    [SerializeField] private float spinSpeed = 90f;
    [SerializeField] private float k = 10f;

    private float ratio = .3f;

    public bool IsFinished { get; private set; } = false;


    [ContextMenu("Reset")]
    public void Reset()
    {
        wheel.rotation = Quaternion.identity;
        spinTime = Random.Range(1, 3);
        spinSpeed = Random.Range(90, 180);
        IsFinished = false;
        buttonStart.enabled = true;
    }

    /// <summary>
    /// Set the ratio of the wheel. Automatically updates appearance.
    /// </summary>
    /// <param name="ratio">Ratio of player's combat level to enemies combat level.</param>
    public void SetRatio(float ratio)
    {
        this.ratio = ratio;
        playerSlice.fillAmount = ratio;
    }

    [ContextMenu("Spin")]
    public void StartSpin()
    {
        buttonStart.enabled = false;
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        float currTime = 0f;

        while (currTime < spinTime)
        {
            currTime += Time.deltaTime;
            float angle = Impulse(2, currTime / spinTime);
            wheel.Rotate(Vector3.forward, angle * spinSpeed);

            if (wheel.rotation.z < 0f)
                wheel.Rotate(0, 0, 360); ;

            yield return null;
        }
        IsFinished = true;
    }

    [ContextMenu("Result")]
    public bool GetResult()
    {
        float rad = Mathf.Deg2Rad * wheel.eulerAngles.z;
        float currAngle = (rad) / (2 * Mathf.PI);
        return currAngle < ratio;
    }

    public void OnEnable()
    {
        buttonStart.enabled = true;
    }

    private static float Impulse(float k, float x)
    {
        float h = k * x;
        return h * Mathf.Exp(1.0f - h);
    }
}


