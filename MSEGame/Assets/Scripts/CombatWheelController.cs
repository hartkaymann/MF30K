using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatWheelController : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private Image playerSlice;
    [SerializeField] private Button buttonStart;

    [SerializeField] private float force = 5000f;
    private Rigidbody2D wheelRb;

    private float ratio = .3f;
    private bool startSpin = false;

    public bool IsFinished { get; private set; } = false;

    private void Start()
    {
        wheelRb = wheel.GetComponent<Rigidbody2D>();
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        wheel.rotation = Quaternion.identity;
        force = Random.Range(1000, 10000);
        IsFinished = false;
        buttonStart.enabled = true;
    }

    private void FixedUpdate()
    {
        if(startSpin)
        {
            wheelRb.AddTorque(force);
            startSpin = false;
        }
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
        IsFinished = false;

        startSpin = true;

        StartCoroutine(WaitForSpin());
    }

    private IEnumerator WaitForSpin()
    {
        yield return new WaitForSeconds(1);

        while (wheelRb.angularVelocity > 0)
        {
            yield return null;
        }
        Debug.Log("Wheel is done.");
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


