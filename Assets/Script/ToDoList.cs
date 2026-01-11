using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

public class TodoItemUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color doneColor = Color.gray;
   
    bool done;

     public void OnDiaryOpened()
    {
        gameObject.SetActive(false); 
    }

  
    public void OnDiaryClosed()
    {
        gameObject.SetActive(true); 
    }
    public void MarkDone()
    {
        if (done) return;
        done = true;

        text.fontStyle = FontStyles.Strikethrough;
        text.color = doneColor;
    }
}