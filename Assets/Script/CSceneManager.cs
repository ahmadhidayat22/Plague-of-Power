using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CutsceneData
{
    public GameObject cutscenePrefab;
    public AudioClip[] voiceClips;
}

public class CSceneManager : MonoBehaviour
{
    [Header("Cutscenes & Audio")]
    public List<CutsceneData> cutscenes = new List<CutsceneData>();
    public AudioSource audioSource;

    [Header("Logo & Transisi")]
    public GameObject logoPanel;
    public GameObject mainMenuPanel; //GameObject tujuan setelah logo
    public float logoDisplayDuration = 3f;

    [Header("Control")]
    public KeyCode skipKey = KeyCode.Space;
    public float delayBetweenScenes = 1f;
    public float initialDelay = 2f; // delay sebelum cutscene pertama

    private bool isSkipping = false;
    private bool hasStartedTransition = false;

    void Start()
    {
        StartCoroutine(PlayCutscenes());
    }

    void Update()
    {
        if (!isSkipping && Input.GetKeyDown(skipKey))
        {
            isSkipping = true;

            if (audioSource != null)
                audioSource.Stop();

            foreach (var c in cutscenes)
                if (c.cutscenePrefab != null)
                    c.cutscenePrefab.SetActive(false);

            StartCoroutine(ShowLogoThenActivateMainMenu());
        }
    }

    IEnumerator PlayCutscenes()
    {
        yield return new WaitForSeconds(initialDelay);
        foreach (var cutscene in cutscenes)
        {
            if (isSkipping) yield break;

            if (cutscene.cutscenePrefab != null)
                cutscene.cutscenePrefab.SetActive(true);

            foreach (var clip in cutscene.voiceClips)
            {
                if (isSkipping) yield break;

                if (clip != null && audioSource != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    yield return new WaitForSeconds(clip.length);
                }
            }

            if (cutscene.cutscenePrefab != null)
                cutscene.cutscenePrefab.SetActive(false);

            yield return new WaitForSeconds(delayBetweenScenes);
        }

        StartCoroutine(ShowLogoThenActivateMainMenu());
    }

    IEnumerator ShowLogoThenActivateMainMenu()
    {
        if (hasStartedTransition) yield break;
        hasStartedTransition = true;

        if (logoPanel != null)
            logoPanel.SetActive(true);

        yield return new WaitForSeconds(logoDisplayDuration);

        if (logoPanel != null)
            logoPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
}
