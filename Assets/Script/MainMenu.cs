using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider BackSound;
    public Slider EffectSound;
    public TextMeshProUGUI txtBackSound;
    public TextMeshProUGUI txtEffectSound;
    public GameObject StartMenu1;
    public GameObject StartMenu2;
    public GameObject StartMenu3;
    public GameObject SettingMenu;
    public GameObject Transparentbackground;
    public GameObject Achievement;
    public GameObject LeaveGame;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemsText;

    const string MapsLevel1 = "Level1";
    const string MapsLevel2 = "Level2";
    const string MapsLevel3 = "Level3";
    const string BackSoundKey = "BackSound";
    const string EffectSoundKey = "EffectSound";

    public void Start()
    {
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();
        gemsText.text = PlayerPrefs.GetInt("Gems").ToString();

        // Set default 70%
        float savedBackSound = PlayerPrefs.HasKey(BackSoundKey) ? PlayerPrefs.GetFloat(BackSoundKey) : 0.1f;
        float savedEffectSound = PlayerPrefs.HasKey(EffectSoundKey) ? PlayerPrefs.GetFloat(EffectSoundKey) : 0.1f;

        BackSound.value = savedBackSound;
        EffectSound.value = savedEffectSound;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        UpdateBackSoundVolume();
        UpdateEffectSoundVolume();
        StartMenu1.SetActive(false);
        StartMenu2.SetActive(false);
        StartMenu3.SetActive(false);
        SettingMenu.SetActive(false);
        Transparentbackground.SetActive(false);
        Achievement.SetActive(false);
        LeaveGame.SetActive(false);
    }

    public void UpdateBackSoundVolume()
    {
        float volumeBS = BackSound.value;
        txtBackSound.text = Mathf.RoundToInt(volumeBS * 100f) + "%";
        PlayerPrefs.SetFloat(BackSoundKey, volumeBS);
        Debug.Log("BackSound volume disimpan: " + volumeBS);
    }

    public void UpdateEffectSoundVolume()
    {
        float volumeSFX = EffectSound.value;
        txtEffectSound.text = Mathf.RoundToInt(volumeSFX * 100f) + "%";
        PlayerPrefs.SetFloat(EffectSoundKey, volumeSFX);
        Debug.Log("EffectSound volume disimpan: " + volumeSFX);
    }


    public void BackFrom(string source)
    {   
        switch (source)
        {
            case "Main":
                Transparentbackground.SetActive(false);
                break;

            case "MapsMenu":
                StartMenu1.SetActive(true);
                StartMenu1.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;

            case "WeaponsMenu":
                StartMenu1.SetActive(true);
                StartMenu1.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;
            default:
                break;
        }
    }

    public void ToDest(string destination){
        switch (destination)
        {   
            case "Start":
                StartMenu1.SetActive(true);
                StartMenu1.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Transparentbackground.SetActive(true);
                break;

            case "MapsMenu":
                StartMenu2.SetActive(false);
                StartMenu1.SetActive(false);
                StartMenu2.SetActive(true);
                StartMenu2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;

            case "WeaponsMenu":
                StartMenu3.SetActive(false);
                StartMenu1.SetActive(false);
                StartMenu3.SetActive(true);
                StartMenu3.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;

            case "StartGameMap1":
                SceneManager.LoadScene(MapsLevel1);
                break;
                
            case "Settings":
                SettingMenu.SetActive(true);
                SettingMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Transparentbackground.SetActive(true);  
                break;

            case "Achievement":
                Achievement.SetActive(true);
                Achievement.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Transparentbackground.SetActive(true);
                break;

            case "Leave":
                LeaveGame.SetActive(true);
                LeaveGame.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Transparentbackground.SetActive(true);
                break;

            case "Exit":
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;

            default:
                break;
        }
    }
}
