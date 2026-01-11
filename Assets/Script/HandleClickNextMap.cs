using UnityEngine;

public class HandleClickNextMap : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject nextMap;
    public AudioSource clickSound;

    private bool clicked = false;

    void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        if (clickSound != null)
            clickSound.Play();

        currentMap.SetActive(false);
        nextMap.SetActive(true);
    }
}
