#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class UpgradeWeaponDataGenerator
{
    [MenuItem("Tools/Generate Upgrade Weapon Data")]
    public static void GenerateUpgradeData()
    {
        var asset = ScriptableObject.CreateInstance<UpgradeWeaponData>();
        asset.weaponName = "P90";
        asset.upgradeLevels = new System.Collections.Generic.List<UpgradeWeaponData.UpgradeLevel>();

        for (int i = 1; i <= 25; i++)
        {
            var levelData = new UpgradeWeaponData.UpgradeLevel
            {
                level = i,
                fireRatePercent = 2f + (i - 1) * 0.9f,         // Naik 0.05 tiap level
                extraDamage = 3 + (i - 1) / 2,                    // Naik tiap 2 level
                extraMaxAmmo = 5 + i ,                             // Bertambah 1 tiap level
                bulletSpeedMultiplier = 2f + (i - 1) * 0.4f,   // Naik 0.02 tiap level
                costCoin = 15 * i + Random.Range(5,10)  // Biaya meningkat linear
            };

            asset.upgradeLevels.Add(levelData);
        }

        string path = "Assets/Prefabs/Weapon/UpgradeWeaponData_P90.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"Upgrade data berhasil dibuat di: {path}");
        Selection.activeObject = asset;
    }
}
#endif
