using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneAfterIntro : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
