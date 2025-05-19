using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------------Audio Source-------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------------Audio Clip-------------")]
    public AudioClip background;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip menu_hoover;
    public AudioClip menu_select;

    // private void Start()
    // {
    //    musicSource.clip = background;
    //    musicSource.Play();
    // }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void Awake()
    {
        if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
