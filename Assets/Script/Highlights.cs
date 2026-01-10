using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteract : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sr;

    public Transform cameraTarget;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private bool isHovered;

    
    private float lastClickTime;
    public float doubleClickTime = 0.25f; // 250 ms

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void Update()
    {
        DetectHover();
        DetectDoubleClick();
    }

    // ================= HOVER =================
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

    // ================= DOUBLE CLICK =================
    void DetectDoubleClick()
    {
        if (!isHovered) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickTime)
            {
                Debug.Log("Double Click detected on " + gameObject.name);
                TeleportCamera();
                lastClickTime = 0f; // reset (anti triple click)
            }
            else
            {
                lastClickTime = Time.time;
            }
        }
    }

   
    void TeleportCamera()
    {
        if (cameraTarget == null) return;

        cam.transform.position = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            cam.transform.position.z
        );

        Debug.Log("Camera TP ke: " + cameraTarget.position);
    }
}
