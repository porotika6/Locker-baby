using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] sentences;
    public float dialogueSpeed = 0.05f;

    private int index = 0;
    private Coroutine typingCoroutine;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartDialogue();
    }

    void Update()
    {
        DetectClickOnThisObject();
    }

    // ================== DIALOG SYSTEM ==================

    public void StartDialogue()
    {
        index = 0;
        dialogueText.text = "";
        StartTyping();
    }

    public void NextSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = sentences[index];
            typingCoroutine = null;
            return;
        }

        index++;

        if (index < sentences.Length)
        {
            StartTyping();
        }
        else
        {
            dialogueText.text = "";
            Debug.Log("Dialogue finished");
        }
    }

    void StartTyping()
    {
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        foreach (char c in sentences[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        typingCoroutine = null;
    }

    // ================== CLICK DETECTION ==================

    void DetectClickOnThisObject()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Debug.Log("Dialogue object clicked");
            NextSentence();
        }
    }
}