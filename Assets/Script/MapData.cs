using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    [Tooltip("nama map harus sama dengan nama scene")]
    public string mapName;
    public bool isShouldBuy;

    public int costBuy;
    public Sprite mapImage;


}
