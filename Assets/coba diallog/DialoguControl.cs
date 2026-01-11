using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemDialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] sentences;
    public float dialogueSpeed = 0.05f;

    int index;
    Coroutine typing;
    bool active;

    Camera cam;

    float lastClickTime;
    public float doubleClickTime = 0.25f;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("Camera.main tidak ditemukan! Pastikan tag MainCamera");

        active = false;

        // Sembunyikan UI di awal
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        DetectDoubleClick();
    }

    // ================= DOUBLE CLICK =================
    void DetectDoubleClick()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            float time = Time.time - lastClickTime;

            if (time <= doubleClickTime)
            {
                HandleInteraction();
                lastClickTime = 0f;
            }
            else
            {
                lastClickTime = Time.time;
            }
        }
    }

    void HandleInteraction()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider == null) return;
        if (hit.collider.gameObject != gameObject) return;

        if (!active)
            StartDialogue();
        else
            NextSentence();
    }

    // ================= DIALOG =================
    void StartDialogue()
    {
        active = true;
        index = 0;

        dialogueText.gameObject.SetActive(true);
        dialogueText.transform.parent.gameObject.SetActive(true);

        StartTyping();
    }

    void NextSentence()
    {
        if (typing != null)
        {
            StopCoroutine(typing);
            dialogueText.text = sentences[index];
            typing = null;
            return;
        }

        index++;

        if (index < sentences.Length)
            StartTyping();
        else
            EndDialogue();
    }

    void StartTyping()
    {
        dialogueText.text = "";
        typing = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        foreach (char c in sentences[index])
    {
        dialogueText.text += c;
        yield return new WaitForSeconds(dialogueSpeed);
    }

    typing = null;

    // ⬇️ JIKA INI KALIMAT TERAKHIR → AUTO END
    if (index == sentences.Length - 1)
    {
        yield return new WaitForSeconds(0.5f); // jeda dikit biar kebaca
        EndDialogue();
    }
    }

    void EndDialogue()
    {
        active = false;

        // SEMBUNYIKAN UI SAJA
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(false);

        // ❌ ITEM TIDAK DIMATIKAN
        // gameObject.SetActive(false);  <-- SUDAH DIHAPUS
    }
}