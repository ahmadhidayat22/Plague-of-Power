using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneVoiceAndSwitch : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip voiceClip;
    public GameObject nextScene;

    [Header("Transisi")]
    public float delayAfterVoice = 1f;
    public Image fadeOverlay;
    public float fadeDuration = 1f;

    [Header("Optional: Skip")]
    public bool allowSkip = false;
    public KeyCode skipKey = KeyCode.Space;

    private bool isSkipping = false;

    void OnEnable()
    {
        StartCoroutine(PlayAndSwitch());
    }

    IEnumerator PlayAndSwitch()
    {
        if (audioSource != null && voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();

            float timer = 0f;
            while (timer < voiceClip.length && !isSkipping)
            {
                timer += Time.deltaTime;

                if (allowSkip && Input.GetKeyDown(skipKey))
                {
                    isSkipping = true;
                    break;
                }

                yield return null;
            }

            audioSource.Stop();
        }

        yield return new WaitForSeconds(delayAfterVoice);

        if (fadeOverlay != null)
            yield return StartCoroutine(FadeOut());

        if (nextScene != null)
            nextScene.SetActive(true);

        gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        Color color = fadeOverlay.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeOverlay.color = color;
            yield return null;
        }
    }
}
