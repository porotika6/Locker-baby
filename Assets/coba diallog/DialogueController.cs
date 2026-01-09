using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] sentences;
    public float dialogueSpeed = 0.05f;

    private int index = 0;
    private Coroutine typingCoroutine;

    void Start()
    {
        StartDialogue();
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
}