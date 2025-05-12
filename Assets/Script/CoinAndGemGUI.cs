using UnityEngine;
using TMPro;

public class CoinAndGemGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemsText;
    [SerializeField] private Player player; // referensi ke script Player
    [SerializeField] private PlayerAimWeapon playerWeapon;
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        if (player != null)
        {
            coinText.text =  player.coinCounter.ToString();
            gemsText.text =  player.gemsCounter.ToString();
            ammoText.text = playerWeapon.ammoText.ToString();
        }
    }
}
