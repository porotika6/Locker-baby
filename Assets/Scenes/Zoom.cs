using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
    public float zoomStep = 1f;
    public float minOrthoSize = 2f;
    public float maxOrthoSize = 10f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            float before = cam.orthographicSize;

            if (Keyboard.current.leftShiftKey.isPressed)
                cam.orthographicSize += zoomStep; // zoom out
            else
                cam.orthographicSize -= zoomStep; // zoom in

            cam.orthographicSize =
                Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);

            Debug.Log($"Zoom click: {before} → {cam.orthographicSize}");
        }
    }
}