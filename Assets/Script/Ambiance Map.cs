using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(AudioSource))]
public class MapAmbienceFade : MonoBehaviour
{
    public float fadeDuration = 0.5f;
 
    private AudioSource audioSource;
    private Coroutine fadeRoutine;
 
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = 0f;
    }
 
    void OnEnable()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        audioSource.Play();
        fadeRoutine = StartCoroutine(Fade(0f, 1f));
    }
 
    void OnDisable()
    {
        if (!gameObject.activeInHierarchy) return;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(Fade(1f, 0f, true));
    }
 
    IEnumerator Fade(float from, float to, bool stopAfter = false)
    {
        float t = 0f;
        audioSource.volume = from;
 
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
 
        audioSource.volume = to;
 
        if (stopAfter)
            audioSource.Stop();

    }

} 