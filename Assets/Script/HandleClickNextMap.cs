using UnityEngine;

public class HandleClickNextMap : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject nextMap;

    private bool clicked = false;

    void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        currentMap.SetActive(false);
        nextMap.SetActive(true);
    }
}
