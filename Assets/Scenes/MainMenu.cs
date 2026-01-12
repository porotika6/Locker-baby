using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject fadePanel;    // Drag UI Panel Hitam ke sini
    public Animator fadeAnimator;   // Drag Animator dari Panel Hitam
    public float fadeDuration = 1.0f;

    public GameObject monologuePanel; // Tarik MonologuePanel ke sini

public void PlayGame()
{
    StartCoroutine(StartGameSequence());
}

IEnumerator StartGameSequence()
{
    // 1. Jalankan Fade Out (Layar jadi hitam)
    fadePanel.SetActive(true);
    fadeAnimator.Play("FadeOut");
    yield return new WaitForSeconds(fadeDuration);

    // 2. Matikan FadePanel, Nyalakan Panel Monolog
    fadePanel.SetActive(false);
    monologuePanel.SetActive(true); 
    // Monolog otomatis jalan karena ada StartDialogue di Start()
}

    public void QuitGame()
    {
        Application.Quit();
    }
}