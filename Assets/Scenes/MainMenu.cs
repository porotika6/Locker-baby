using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject fadePanel;    // Drag UI Panel Hitam ke sini
    public Animator fadeAnimator;   // Drag Animator dari Panel Hitam
    public float fadeDuration = 1.0f;

   

public void StartGame()
{
    SceneManager.LoadSceneAsync(1);
    StartCoroutine(LoadLevelDirectly());
}

IEnumerator LoadLevelDirectly()
{
    fadeAnimator.Play("FadeOut"); // Layar jadi hitam
    yield return new WaitForSeconds(fadeDuration);
    SceneManager.LoadScene("Trial Area 1"); // Langsung pindah
}

    public void QuitGame()
    {
        Application.Quit();
    }
}