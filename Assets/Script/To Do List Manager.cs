using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem; // Wajib untuk sistem input baru

public class TodoManager : MonoBehaviour
{
    public static TodoManager Instance;

    [Header("UI Header")]
    public TextMeshProUGUI headerText;
    public Color headerDoneColor = Color.gray;

    [Header("Transition Settings")]
    public string targetSceneName;
    public GameObject fadePanel;    
    public Animator fadeAnimator;   
    public float fadeDuration = 0.5f;

    [Header("Monologue Settings (System Baru)")]
    public GameObject monologuePanel; 
    public TextMeshProUGUI monologueText; 
    public string[] sentences; 
    public float typingSpeed = 0.05f;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool monologueActive = false;

    [Header("Quest List")]
    public List<TodoItemUI> allTasks = new List<TodoItemUI>();

    void Awake() { Instance = this; }

    void Start()
    {
        // Memulai urutan scene: Cek monolog dulu
        if (sentences != null && sentences.Length > 0)
        {
            StartCoroutine(BeginSceneWithMonologue());
        }
        else
        {
            // Jika tidak ada monolog, langsung Fade In
            StartCoroutine(JustFadeIn());
        }
    }

    void Update()
    {
        // Deteksi Klik menggunakan Input System Baru
        if (monologueActive && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                monologueText.text = sentences[dialogueIndex];
                isTyping = false;
            }
            else
            {
                NextSentence();
            }
        }
    }

    // ================== LOGIKA MONOLOG BARU ==================

    IEnumerator BeginSceneWithMonologue()
    {
        monologueActive = true;
        monologuePanel.SetActive(true);
        if (fadePanel != null) fadePanel.SetActive(false); 
        
        dialogueIndex = 0;
        StartCoroutine(TypeSentence());
        yield return null;
    }

    void NextSentence()
    {
        dialogueIndex++;
        if (dialogueIndex < sentences.Length)
        {
            StartCoroutine(TypeSentence());
        }
        else
        {
            // Monolog Selesai -> Masuk ke Gameplay (Fade In)
            monologueActive = false;
            monologuePanel.SetActive(false);
            StartCoroutine(JustFadeIn());
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        monologueText.text = "";
        foreach (char c in sentences[dialogueIndex])
        {
            monologueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    IEnumerator JustFadeIn()
    {
        if (fadePanel != null && fadeAnimator != null)
        {
            fadePanel.SetActive(true);
            fadeAnimator.Play("FadeIn"); 
            yield return new WaitForSeconds(fadeDuration);
            fadePanel.SetActive(false);
        }
    }

    // ================== LOGIKA TRANSISI (JANGAN DIGANTI) ==================

    public IEnumerator TriggerFadeSequence(System.Action onMidFade)
    {
        if (fadePanel != null && fadeAnimator != null)
        {
            fadePanel.SetActive(true);
            fadeAnimator.Play("FadeOut");
            yield return new WaitForSeconds(fadeDuration);
            onMidFade?.Invoke();
            fadeAnimator.Play("FadeIn");
            yield return new WaitForSeconds(fadeDuration);
            fadePanel.SetActive(false);
        }
        else
        {
            onMidFade?.Invoke();
        }
    }

    // ================== LOGIKA QUEST & END LEVEL ==================

    public void CheckGlobalProgress()
    {
        bool allDone = true;
        foreach (var task in allTasks)
        {
            if (task == null || !task.isCompleted) { allDone = false; break; }
        }
        if (allDone) { StartCoroutine(EndLevelSequence()); }
    }

    IEnumerator EndLevelSequence()
    {
        if (headerText != null)
        {
            headerText.fontStyle = FontStyles.Strikethrough;
            headerText.color = headerDoneColor;
        }
        yield return new WaitForSeconds(1.0f);
        if (fadePanel != null && fadeAnimator != null)
        {
            fadePanel.SetActive(true);
            fadeAnimator.Play("FadeOut");
        }
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(targetSceneName);
    }
}