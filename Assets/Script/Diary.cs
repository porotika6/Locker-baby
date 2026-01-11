using UnityEngine;

public class Diary : MonoBehaviour
{
    public TodoItemUI[] todoItems; // assign semua TodoItemUI di Inspector

    public bool isOpen = false;

    public void OpenDiary()
    {
        isOpen = true;

        // Panggil semua TodoItemUI untuk sembunyikan
        foreach (var item in todoItems)
        {
            item.OnDiaryOpened();
        }

        // Highlight tetap jalan seperti biasa
        // ... kode Highlight di sini ...
    }

    public void CloseDiary()
    {
        isOpen = false;

        // Tampilkan kembali ToDoList jika perlu
        foreach (var item in todoItems)
        {
            item.OnDiaryClosed();
        }
    }
}