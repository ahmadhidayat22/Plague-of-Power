using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public MapData[] mapData;
    public Transform MapListParent;
    public GameObject MapItemPrefab;
    public TextMeshProUGUI gemsText;

    void Start()
    {
        CreateMapUI();
    }

    void CreateMapUI()
    {
        int gemsPlayer = PlayerPrefs.GetInt("Gems");
        for (int i = 0; i < mapData.Length; i++)
        {
            MapData map = mapData[i];

            GameObject item = Instantiate(MapItemPrefab, MapListParent);
            item.SetActive(true);

            Image mapImage = item.transform.Find("Viewport/Image").GetComponent<Image>();
            if (map.mapImage != null)
                mapImage.sprite = map.mapImage;

            TextMeshProUGUI mapName = item.transform.Find("Viewport/Image/MapText").GetComponent<TextMeshProUGUI>();

            Button buyButton = item.transform.Find("btnBuy").GetComponent<Button>();
            Button playButton = item.transform.Find("btnPlay").GetComponent<Button>();
            TextMeshProUGUI costBuy = item.transform.Find("btnBuy/CostText").GetComponent<TextMeshProUGUI>();

            mapName.text = $"{map.mapName}";
            costBuy.text = $"{map.costBuy}";
            if (map.isShouldBuy)
            {
                buyButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
            }
            if (gemsPlayer < map.costBuy)
            {
                buyButton.interactable = false;
            }

            buyButton.onClick.AddListener(() =>
            {
                gemsPlayer -= map.costBuy;
                PlayerPrefs.SetInt("Gems", gemsPlayer);
                gemsText.text = gemsPlayer.ToString();
                map.isShouldBuy = false;
                RefreshUI();
            });

            playButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(map.mapName);
            });
        }
    }

    void RefreshUI()
    {

        foreach (Transform child in MapListParent)
        {
            Destroy(child.gameObject);
        }
        CreateMapUI();
    }

}
