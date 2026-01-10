using UnityEngine;

public class CameraAudio_Bayi : MonoBehaviour
{
    public AudioClip areaMusic;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            AudioManager.Instance.PlayBGM(areaMusic);
        }
    }
}
