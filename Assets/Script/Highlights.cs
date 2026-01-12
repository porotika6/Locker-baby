using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ItemInteract : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sr;

    public Transform cameraTarget;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private bool isHovered;

    private float lastClickTime;
    public float doubleClickTime = 0.25f;
    public Sprite HoverSprite;
    private Sprite DefaultSprite;

    // 🔥 EVENT (INI KUNCI)
    public UnityEvent OnDoubleClick;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        DefaultSprite = sr.sprite;
    }

    void Update()
    {
        DetectHover();
        DetectDoubleClick();
    }

    void DetectHover()
    {
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isHovered)
            {
                isHovered = true;
                sr.sprite = HoverSprite;
            }
        
        }
        else if (isHovered)
        {
            isHovered = false;
            sr.sprite = DefaultSprite;
        }
    }

    void DetectDoubleClick()
    {
        if (!isHovered) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Time.time - lastClickTime <= doubleClickTime)
            {
                TeleportCamera();

                // 🔔 KIRIM SINYAL
                OnDoubleClick?.Invoke();

                lastClickTime = 0f;
            }
            else
            {
                lastClickTime = Time.time;
            }
        }
    }

    public void TeleportCamera()
    {
        if (cameraTarget == null) return;

        cam.transform.position = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            cam.transform.position.z
        );
    }
     
}
