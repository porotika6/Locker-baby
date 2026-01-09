using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    private SpriteRenderer sr;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = normalColor;
    }

    void OnMouseEnter()
    {
        sr.color = highlightColor;
        Debug.Log("Mouse masuk ke item: " + gameObject.name);
    }

    void OnMouseExit()
    {
        sr.color = normalColor;
        Debug.Log("Mouse keluar dari item: " + gameObject.name);
    }
}