using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private string songToPlay;

    public void Play()
    {
        AudioManager.instance.Play(songToPlay);
    }
}
