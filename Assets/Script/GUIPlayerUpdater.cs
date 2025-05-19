using UnityEngine;
using TMPro;

public class CoinAndGemGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemsText;
    [SerializeField] private Player player; // referensi ke script Player
    [SerializeField] private GunShoot playerWeapon;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text medkitText;
    [SerializeField] private TMP_Text HealthText;

    private void Update()
    {
        if (player != null)
        {
            coinText.text =  player.GetCoin.ToString();
            gemsText.text =  player.GetGems.ToString();
            ammoText.text = playerWeapon.ammoText.ToString();
            medkitText.text = player.GetMedkit.ToString();
            HealthText.text = player.getCurrentHealth.ToString() + " / " + player.getMaxHealth.ToString();
        }
    }
}
