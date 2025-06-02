using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("--------------Audio Source-------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioSource GetSFXSource => SFXSource;

    [Header("--------------Audio Clip-------------")]
    public AudioClip background;
    public AudioClip heal;
    public AudioClip menu_hoover;
    public AudioClip menu_select;
    public AudioClip walk;

    [Header("--------------Volume Sliders-------------")]
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;

    const string BackSoundKey = "BackSound";
    const string EffectSoundKey = "EffectSound";
    public TextMeshProUGUI txtBackSound;
    public TextMeshProUGUI txtEffectSound;

    private void Awake()
    {
        // Singleton pattern
        //if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        // Mulai background music
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void Start()
    {
        // Ambil volume dari PlayerPrefs atau set default 0.1
        float savedBackSound = PlayerPrefs.HasKey(BackSoundKey) ? PlayerPrefs.GetFloat(BackSoundKey) : 0.1f;
        float savedEffectSound = PlayerPrefs.HasKey(EffectSoundKey) ? PlayerPrefs.GetFloat(EffectSoundKey) : 0.1f;

        // Set nilai slider dan sumber audio
        music.value = savedBackSound;
        sfx.value = savedEffectSound;
        musicSource.volume = savedBackSound;
        SFXSource.volume = savedEffectSound;

        // Listener agar volume bisa langsung berubah saat slider digeser
        music.onValueChanged.AddListener(delegate { UpdateBackSoundVolume(); });
        sfx.onValueChanged.AddListener(delegate { UpdateEffectSoundVolume(); });

        // Panggil fungsi update saat start
        UpdateBackSoundVolume();
        UpdateEffectSoundVolume();
    }

    public void UpdateBackSoundVolume()
    {
        float volumeBS = music.value;
        musicSource.volume = volumeBS;
        txtBackSound.text = Mathf.RoundToInt(volumeBS * 100f) + "%";
        PlayerPrefs.SetFloat(BackSoundKey, volumeBS);
        Debug.Log("BackSound volume disimpan: " + Mathf.RoundToInt(volumeBS * 100f) + "%");
    }

    public void UpdateEffectSoundVolume()
    {
        float volumeSFX = sfx.value;
        SFXSource.volume = volumeSFX;
        txtEffectSound.text = Mathf.RoundToInt(volumeSFX * 100f) + "%";
        PlayerPrefs.SetFloat(EffectSoundKey, volumeSFX);
        Debug.Log("EffectSound volume disimpan: " + Mathf.RoundToInt(volumeSFX * 100f) + "%");

        GunShoot gunShoot = FindObjectOfType<GunShoot>();
        if (gunShoot != null)
        {
            gunShoot.UpdateSFXVolume(volumeSFX);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
