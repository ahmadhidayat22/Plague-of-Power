using UnityEditor.SearchService;
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

    private void Awake()
    {
        playerControls = new Player_controls();
        
    }

    public void Action(string action)
    {
        switch (action)
        {
            case "Pause":
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Game Paused");
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;

            case "Resume":
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                Vector2 hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
                Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
                Debug.Log("Game Resumed");
                break;

            case "Restart":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case "OpenSettings":
                Settings.SetActive(true);
                Debug.Log("Settings Opened");
                break;

            case "CloseSettings":
                Settings.SetActive(false);
                Debug.Log("Settings Closed");
                break;

            case "Exit":
                playerControls.Movement.Disable();
                playerControls.Combat.Disable();
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
                break;

            default:
                Debug.Log("Invalid action");
                break;
        }
    }
}
