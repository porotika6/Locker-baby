using UnityEngine;
using TMPro;

public class TodoItemUI : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public Color completedColor = Color.gray;
    
    [HideInInspector] public bool isCompleted = false;

    public void MarkAsDone()
    {
        if (isCompleted) return;
        
        isCompleted = true;
        taskText.fontStyle = FontStyles.Strikethrough;
        taskText.color = completedColor;

        // Beritahu manager untuk cek status global
        TodoManager.Instance.CheckGlobalProgress();
    }
}