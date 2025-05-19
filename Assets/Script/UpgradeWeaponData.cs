using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeWeaponData", menuName = "Weapon/UpgradeWeaponData")]
public class UpgradeWeaponData : ScriptableObject
{
    public string weaponName;
    public List<UpgradeLevel> upgradeLevels;

    public int MaxLevel => upgradeLevels.Count;
    [System.Serializable]
    public class UpgradeLevel
    {
        public int level;
        public float fireRatePercent = 1f;
        public int extraDamage = 0;
        public int extraMaxAmmo = 0;
        public float bulletSpeedMultiplier = 1f;
        public int costCoin = 1;
    }

    public UpgradeLevel GetUpgradeData(int level)
    {
        if (level <= 0 || level > upgradeLevels.Count)
            return null;

        return upgradeLevels[level - 1]; // karena index array mulai dari 0
    }
    

    public bool IsMaxLevel(int currentLevel)
    {
        return currentLevel >= MaxLevel;
    }

}
