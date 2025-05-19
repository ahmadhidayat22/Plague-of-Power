using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public float delayTime = 3f; // waktu tampil

    void Start()
    {
        Invoke("LoadNextScene", delayTime);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu"); // ganti sesuai nama scene berikutnya
    }
}
