using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public float wait_time = 7f;
    void Start()
    {
        StartCoroutine(wait_intro());
    }

    IEnumerator wait_intro()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.LoadScene(1);
    }
}
