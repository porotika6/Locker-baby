using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteract : MonoBehaviour
{
   
   private Camera cam;
    private SpriteRenderer sr;

    public Transform cameraTarget; // titik tujuan kamera

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private bool isHovered;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void Update()
    {
        DetectHover();
        DetectClick();
    }

    void DetectHover()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isHovered)
            {
                isHovered = true;
                sr.color = highlightColor;
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                sr.color = normalColor;
            }
        }
    }

    void DetectClick()
    {
        if (isHovered && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TeleportCamera();
        }
    }

    void TeleportCamera()
    {
        if (cameraTarget == null) return;

        cam.transform.position = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            cam.transform.position.z // Z kamera tetap
        );

        Debug.Log("Camera TP ke: " + cameraTarget.position);
    }
}