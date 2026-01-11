using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ScreenFadeManager : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Transition")]
    public float transitionDuration = 0.6f;
    public float doubleClickTime = 0.3f;

    [Header("Camera (Optional)")]
    public Camera camA;
    public Camera camB;

    float lastClickTime;
    bool isTransitioning;

    bool waitingForSecondClick;

void Update()
{
    if (isTransitioning) return;

    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        if (!waitingForSecondClick)
        {
            waitingForSecondClick = true;
            lastClickTime = Time.time;
        }
        else
        {
            if (Time.time - lastClickTime <= doubleClickTime)
            {
                waitingForSecondClick = false;
                StartTransition(); // ✅ CUMA SEKALI
            }
            else
            {
                lastClickTime = Time.time;
            }
        }
    }

    // reset kalau timeout
    if (waitingForSecondClick && Time.time - lastClickTime > doubleClickTime)
    {
        waitingForSecondClick = false;
    }
}
    void StartTransition()
    {
        PlayTransition(() =>
        {
            if (camA != null && camB != null)
            {
                camA.gameObject.SetActive(false);
                camB.gameObject.SetActive(true);
            }
        });
    }

    public void PlayTransition(System.Action onMiddle)
    {
        if (!isTransitioning)
            StartCoroutine(TransitionRoutine(onMiddle));
    }

    IEnumerator TransitionRoutine(System.Action onMiddle)
    {
        isTransitioning = true;

        animator.SetTrigger("Play");

        yield return new WaitForSeconds(transitionDuration / 2f);
        onMiddle?.Invoke();
        yield return new WaitForSeconds(transitionDuration / 2f);

        isTransitioning = false;
    }
}