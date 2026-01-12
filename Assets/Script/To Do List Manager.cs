using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TodoManager : MonoBehaviour
{
    public static TodoManager Instance;

    [Header("Main Header Settings")]
    public TextMeshProUGUI headerText;
    public Color headerDoneColor = Color.gray;

    [Header("List of All Task UI")]
    public List<TodoItemUI> allTasks;

    void Awake()
    {
        Instance = this;
    }

    public void CheckGlobalProgress()
    {
        bool allDone = true;

        foreach (var task in allTasks)
        {
            if (!task.isCompleted)
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            headerText.fontStyle = FontStyles.Strikethrough;
            headerText.color = headerDoneColor;
            Debug.Log("Semua misi selesai!");
        }
    }
}