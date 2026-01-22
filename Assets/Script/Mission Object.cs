using UnityEngine;
using UnityEngine.InputSystem;

public class MissionObject : MonoBehaviour
{
    public TodoItemUI linkedUI; // Tarik teks misinya ke sini di Inspector
    private Camera cam;
    

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Deteksi klik kiri menggunakan New Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectClick();
        }
    }

    void DetectClick()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

       if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            CompleteThisMission();
        }
    }

    void CompleteThisMission()
    {
       if (linkedUI != null)
        {
            linkedUI.MarkAsDone(); 
        }

        // Objek ini menghilang
        gameObject.SetActive(false);
    }
}