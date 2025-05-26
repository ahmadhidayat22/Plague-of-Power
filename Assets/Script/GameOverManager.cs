using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private bool isNewHighScore = false;
    private float hue = 0f;

    public void ShowGameOver(float currentScore)
    {
        int scoreInt = Mathf.FloorToInt(currentScore);
        scoreText.text = $"Score: {scoreInt}";

        int best = PlayerPrefs.GetInt("HighScore", 0);
        isNewHighScore = false;

        if (scoreInt > best)
        {
            PlayerPrefs.SetInt("HighScore", scoreInt);
            PlayerPrefs.Save();
            isNewHighScore = true;
        }

        UpdateHighScoreText();

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isNewHighScore)
        {
            hue += Time.deltaTime * 0.5f; 
            if (hue > 1f) hue -= 1f;
            Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
            string colorHex = ColorUtility.ToHtmlStringRGB(rainbowColor);

            int highScoreValue = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = $"<color=#{colorHex}>High Score: {highScoreValue} NEW</color>";
        }
    }

    private void UpdateHighScoreText()
    {
        if (!isNewHighScore)
        {
            int highScoreValue = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = $"High Score: {highScoreValue}";
        }
    }
}
