using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemDialogueController : MonoBehaviour
{
    [Header("Dialogue")]
    public TextMeshProUGUI dialogueText;
    public string[] sentences;
    public float dialogueSpeed = 0.05f;

    [Header("Transition")]
    public Animator fadeAnimator;
    public float fadeDuration = 1f;

    [Header("Movement")]
    public Transform cameraTarget;   // Camera Holder / Player
    public Transform targetPosition; // Posisi tujuan

    [Header("Input")]
    public float doubleClickTime = 0.25f;

    int index;
    Coroutine typing;
    bool active;
    bool isTransitioning;

    Camera cam;
    float lastClickTime;

    // ================= UNITY =================
    void Start()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogError("Camera.main tidak ditemukan! Pastikan tag MainCamera");

        active = false;
        isTransitioning = false;

        // Sembunyikan UI di awal
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        DetectDoubleClick();
    }

    // ================= INPUT =================
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

        TriggerInteraction(); // public event
    }

    // ================= PUBLIC EVENT =================
    public void TriggerInteraction()
    {
        if (active || isTransitioning) return;
        StartCoroutine(TransitionThenDialogue());
    }

    // ================= TRANSITION =================
    IEnumerator TransitionThenDialogue()
    {
        isTransitioning = true;
        active = true;

        // Fade Out
        if (fadeAnimator != null)
            fadeAnimator.Play("FadeOut", -1, 0f);

        yield return new WaitForSeconds(fadeDuration);

        // Pindah posisi
        if (cameraTarget != null && targetPosition != null)
        {
            cameraTarget.position = new Vector3(
                targetPosition.position.x,
                targetPosition.position.y,
                cameraTarget.position.z
            );
        }

        // Fade In
        if (fadeAnimator != null)
            fadeAnimator.Play("FadeIn", -1, 0f);

        yield return new WaitForSeconds(fadeDuration);

        StartDialogue();
        isTransitioning = false;
    }

    // ================= DIALOGUE =================
    void StartDialogue()
    {
        index = 0;

        dialogueText.text = "";
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
    }

    void EndDialogue()
    {
        active = false;

        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(false);
    }
}
