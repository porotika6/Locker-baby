using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events; // TAMBAHKAN INI PENTING!

public class KeypadSystem : MonoBehaviour
{
    [Header("Settings")]
    // Dengan 'public', password ini akan muncul di Inspector Unity
    public string correctPassword = "1234"; 
    public Camera cam;
    public int maxLimit = 4;
    [Header("UI Elements")]
    public TextMeshProUGUI displayText; 

    [Header("Event Saat Benar")]
    // Ini adalah slot untuk menghubungkan ke script ItemInteract
    public UnityEvent OnCorrectPassword; 

    private string currentInput = ""; 
     public Transform cameraTarget;

    void Start()
    {
        UpdateDisplay();
    }

    public void PressButton(string number)
    {
        if (currentInput.Length < maxLimit)
        {
            currentInput += number;
            UpdateDisplay();
            
            // Cek otomatis jika digit sudah penuh
            if(currentInput.Length == maxLimit)
            {
                CheckPassword();
            }
        }
    }

    void UpdateDisplay()
    {
        displayText.text = currentInput;
    }

   public void CheckPassword()
    {
        if (currentInput == correctPassword)
        {
            Debug.Log("Password Benar!");
            displayText.color = Color.green;
            OnCorrectPassword?.Invoke(); 
            
            // Tambahkan ini agar layar bersih kembali setelah 1 detik
            Invoke("ResetInput", 1.0f); 
        }
        else
        {
            Debug.Log("Password Salah!");
            displayText.color = Color.red; 
            Invoke("ResetInput", 1f); 
        }
    }

    public void ResetInput()
    {
        currentInput = "";
        displayText.text = "";
        displayText.color = Color.white; 
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