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

    
    
    public void Start()
    {
        // Set default 70%
        BackSound.value = 0.7f;
        EffectSound.value = 0.7f;

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
        float volume = BackSound.value;
        txtBackSound.text = Mathf.RoundToInt(volume * 100f) + "%";
    }

    public void UpdateEffectSoundVolume()
    {
        float volume = EffectSound.value;
        txtEffectSound.text = Mathf.RoundToInt(volume * 100f) + "%";
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
                SceneManager.LoadScene("SampleScene");
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
