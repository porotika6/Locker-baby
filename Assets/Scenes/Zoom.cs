using System.Runtime.CompilerServices;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoomSize = 2f;
    public float maxZoomSize = 10f;

    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0f)
        {
            float newSize = cam.orthographicSize - scrollInput * zoomSpeed * Time.deltaTime * 50;

            cam.orthographicSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);
        }

    }
}
