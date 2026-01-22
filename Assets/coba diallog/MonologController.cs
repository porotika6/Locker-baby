using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] sentences;
    public float dialogueSpeed = 0.05f;
    public string nextSceneName; // Nama scene tujuan setelah monolog

    private int index = 0;
    private Coroutine typingCoroutine;
    private Camera cam;
    private bool isFinished = false;

    void Start()
    {
        cam = Camera.main;
        // Panel monolog biasanya dimulai dalam keadaan aktif 
        // setelah dipanggil dari Main Menu
        StartDialogue();
    }

    void Update()
    {
        if (!isFinished)
        {
            DetectClickOnThisObject();
        }
    }

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
            EndMonologue();
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

    void DetectClickOnThisObject()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        // Klik di mana saja pada layar (menggunakan raycast ke objek collider full screen)
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        NextSentence();
    }
    }

    void EndMonologue()
    {
        isFinished = true;
        dialogueText.text = "";
        // Langsung pindah ke scene game
        SceneManager.LoadScene(nextSceneName);
    }
}