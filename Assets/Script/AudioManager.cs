using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Tambahkan ini untuk mengakses komponen UI

public class AudioManager : MonoBehaviour
{
    [Header("--------------Audio Source-------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------------Audio Clip-------------")]
    public AudioClip background;
    //public AudioClip shoot;
    //public AudioClip reload;
    public AudioClip heal;
    public AudioClip menu_hoover;
    public AudioClip menu_select;

    // Tambahkan referensi untuk slider
    [Header("--------------Volume Sliders-------------")]
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    private void Awake()
    {
        if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Inisialisasi volume musik dan SFX saat game dimulai
        // Jika ada slider, atur nilai slider sesuai volume awal dan sebaliknya
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicSource.volume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = SFXSource.volume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Memulai musik background saat Awake, jika belum ada Start() yang mengaturnya
        musicSource.clip = background;
        musicSource.loop = true; // Pastikan musik berulang
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Metode untuk mengatur volume musik
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Metode untuk mengatur volume SFX
    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}