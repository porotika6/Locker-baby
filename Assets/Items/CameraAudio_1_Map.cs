using UnityEngine;

public class CameraAudio_1_Map
    : MonoBehaviour
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
