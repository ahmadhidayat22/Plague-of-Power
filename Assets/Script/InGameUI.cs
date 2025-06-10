// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject Settings;
    private Player_controls playerControls;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    AudioManager audioManager;

    private void Awake()
    {
        playerControls = new Player_controls();
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj == null)
        {
            Debug.LogError("AudioManager object with tag 'Audio' not found!");
            return;
        }

        audioManager = audioObj.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager component not found on the tagged object!");
        }
    }

    public void Action(string action)
    {
        switch (action)
        {
            case "Pause":
                audioManager.PlaySFX(audioManager.menu_select);
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Game Paused");
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;

            case "Resume":
                audioManager.PlaySFX(audioManager.menu_select);
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                Vector2 hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
                Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
                Debug.Log("Game Resumed");
                break;

            case "Restart":
                audioManager.PlaySFX(audioManager.menu_select);
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case "OpenSettings":
                audioManager.PlaySFX(audioManager.menu_select);
                Settings.SetActive(true);
                Debug.Log("Settings Opened");
                break;

            case "CloseSettings":
                audioManager.PlaySFX(audioManager.menu_select);
                Settings.SetActive(false);
                Debug.Log("Settings Closed");
                break;

            case "Exit":
                audioManager.PlaySFX(audioManager.menu_select);
                playerControls.Movement.Disable();
                playerControls.Combat.Disable();
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    Player player = playerObj.GetComponent<Player>();
                    player.setPlayerPrefsCoinandGems();
                }                
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
                break;

            default:
                Debug.Log("Invalid action");
                break;
        }
    }
}
