using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

// Tambahkan ini untuk Input System yang baru
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
    public float fadeDuration = 1.0f; // Sesuaikan dengan durasi animasi kamu

    [Header("Monologue Settings")]
    public GameObject monologuePanel; // Panel Hitam tempat teks
    public TextMeshProUGUI monologueText; // Komponen teks monolog
    public string[] sentences; // Isi monolog untuk scene ini
    public float typingSpeed = 0.05f;
    private int dialogueIndex = 0;
    private bool isTyping = false;
    private bool monologueActive = false;

    [Header("Quest List")]
    public List<TodoItemUI> allTasks = new List<TodoItemUI>();

    void Awake() { Instance = this; }
    void Start()
{
    ResetTodoList();
    StartCoroutine(JustFadeIn());
}

void ResetTodoList()
{
    foreach (var task in allTasks)
    {
        if (task == null) continue;

        task.isCompleted = false;
        // task.UpdateUI(); // pastikan checkbox / text reset
    }
}

public void UpdateUI()
{
    // checkmark.SetActive(isCompleted);
}
    
    void Update()
    {
        
        bool clickedThisFrame = false;

    #if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
  
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            clickedThisFrame = true;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            clickedThisFrame = true;
        }
    #else
        // Fallback ke Input lama (untuk proyek yang masih pakai legacy)
        if (Input.GetMouseButtonDown(0))
        {
            clickedThisFrame = true;
        }
    #endif

        // Deteksi klik untuk lanjut monolog
        if (monologueActive && clickedThisFrame)
        {
            if (isTyping)
            {
                // Jika sedang mengetik, klik akan langsung memunculkan semua teks
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

    // ================== LOGIKA MONOLOG ==================
    
    public void StartMonologue()
{
    if (sentences == null || sentences.Length == 0) return;

    StopAllCoroutines();
    StartCoroutine(BeginSceneWithMonologue());
}
    IEnumerator BeginSceneWithMonologue()
    {
        monologueActive = true;
        if (monologuePanel != null) monologuePanel.SetActive(true);
        if (fadePanel != null) fadePanel.SetActive(false); // Pastikan panel transisinya mati dulu

        dialogueIndex = 0;
        yield return StartCoroutine(TypeSentence());
    }

    IEnumerator FadeOutAndLoadScene()
{
    if (fadePanel != null)
        fadePanel.SetActive(true);

    if (fadeAnimator != null)
        fadeAnimator.Play("FadeOut", -1, 0f); // ⬅️ NAMA ANIMASI

    yield return new WaitForSeconds(fadeDuration);

    SceneManager.LoadScene(targetSceneName);
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
    monologueActive = false;
    monologuePanel.SetActive(false);

    // LANGSUNG FADE OUT & PINDAH SCENE
    StartCoroutine(FadeOutAndLoadScene());
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

    // ================== LOGIKA TRANSISI SCENE & QUEST ==================

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

    public void CheckGlobalProgress()
   {
    bool allDone = true;
    foreach (var task in allTasks)
    {
        if (task == null || !task.isCompleted)
        {
            allDone = false;
            break;
        }
    }

    if (allDone)
    {
        StartMonologue(); // ⬅️ MONOLOG DI SINI
    }
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
            fadeAnimator.Play("FadeOut", -1, 0f);
        }

        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(targetSceneName);
    }
}