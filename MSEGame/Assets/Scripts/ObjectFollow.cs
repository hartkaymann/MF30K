using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    [HideInInspector] public Transform Follow;

    private Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Follow == null)
            return;

        var screenPos = MainCamera.WorldToScreenPoint(Follow.position);

        transform.position = screenPos;
    }
}
