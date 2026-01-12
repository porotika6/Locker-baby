using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class ItemInteract : MonoBehaviour
{
    [Header("Mission Settings")]
    public TodoItemUI linkedUI; // Tarik TodoItemUI misi ke sini
    public bool destroyOnComplete = true; // Apakah item hilang setelah diklik?

    [Header("Interaction Settings")]
    private Camera cam;
    private SpriteRenderer sr;
    public Transform cameraTarget; // Target kamera jika ingin pindah posisi
    public Sprite hoverSprite;
    private Sprite defaultSprite;
    
    private bool isHovered;
    private float lastClickTime;
    public float doubleClickTime = 0.25f;

    [Header("Events")]
    public UnityEvent OnDoubleClick;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) defaultSprite = sr.sprite;
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
                if (hoverSprite != null) sr.sprite = hoverSprite;
            }
        }
        else if (isHovered)
        {
            isHovered = false;
            if (sr != null) sr.sprite = defaultSprite;
        }
    }

    void DetectDoubleClick()
    {
        if (!isHovered) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Time.time - lastClickTime <= doubleClickTime)
            {
                // Menjalankan urutan transisi
                StartCoroutine(ExecuteInteraction());
                lastClickTime = 0f;
            }
            else
            {
                lastClickTime = Time.time;
            }
        }
    }

    IEnumerator ExecuteInteraction()
    {
        // 1. Layar Hitam (Fade Out)
        if (TodoManager.Instance != null)
            yield return StartCoroutine(TodoManager.Instance.TriggerFadeSequence(() => {
                
                // 2. LOGIKA MISI (Dijalankan saat layar sedang hitam)
                if (linkedUI != null)
                {
                    linkedUI.MarkAsDone(); // Mencoret teks di To Do List
                }

                // 3. Pindah Kamera (Jika ada target)
                TeleportCamera();

                // 4. Jalankan event tambahan dari Inspector
                OnDoubleClick?.Invoke();

                // 5. Menghilangkan Item
                if (destroyOnComplete)
                {
                    gameObject.SetActive(false);
                }
            }));
    }

    public void TeleportCamera()
    {
        if (cameraTarget == null) return;
        cam.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, cam.transform.position.z);
    }
}