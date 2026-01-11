using UnityEngine;

public class HandleClickNextMap : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject nextMap;

    public AudioSource audioSource;   // AudioSource
    public AudioClip transitionSound; // MP3 / WAV

    private bool clicked = false;

    void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        // delay biar suara kedengeran dulu
        Invoke(nameof(SwitchMap), 0.2f);
    }

    void SwitchMap()
    {
        currentMap.SetActive(false);
        nextMap.SetActive(true);
    }
}
