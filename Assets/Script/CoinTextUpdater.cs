using UnityEngine;
using TMPro;

public class CoinTextUpdater : MonoBehaviour
{
    private TMP_Text coinText;

    void Start()
    {
        coinText = GetComponent<TMP_Text>();

        if (!PlayerPrefs.HasKey("Coin"))
            PlayerPrefs.SetInt("Coin", 0);
    }

    void Update()
    {
        int coin = PlayerPrefs.GetInt("Coin");
        coinText.text = coin.ToString();
    }

    public void CoinsTextUpdater(int amount)
    {
        coinText.text = amount.ToString();

    }
}
