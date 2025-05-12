using UnityEngine;
using TMPro;

public class GemTextUpdater : MonoBehaviour
{
    private TMP_Text gemText;

    void Start()
    {
        gemText = GetComponent<TMP_Text>();

        if (!PlayerPrefs.HasKey("Gems"))
            PlayerPrefs.SetInt("Gems", 0);
    }

    void Update()
    {
        int gem = PlayerPrefs.GetInt("Gems");
        gemText.text = gem.ToString();
    }

    public void GemsTextUpdater(int amount)
    {
        gemText.text = amount.ToString();

    }
}
