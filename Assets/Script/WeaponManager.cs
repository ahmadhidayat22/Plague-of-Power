using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] allWeapons;
    private int currentWeaponIndex = 0;
    public Transform weaponHolder;

    private GameObject currentWeaponGO;
    // private PlayerCurrency playerCurrency;

    void Start()
    {
        // playerCurrency = FindObjectOfType<PlayerCurrency>();
        EquipWeapon(0); // default
    }

    public void EquipWeapon(int index)
    {
        if (currentWeaponGO != null)
            Destroy(currentWeaponGO);

        currentWeaponIndex = index;
        currentWeaponGO = Instantiate(allWeapons[index].weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
    }


    // for buy weapon
    // public bool TryBuyWeapon(int index)
    // {
    //     Weapon weaponToBuy = allWeapons[index];
    //     if (playerCurrency.coin >= weaponToBuy.cost)
    //     {
    //         playerCurrency.coin -= weaponToBuy.cost;
    //         EquipWeapon(index);
    //         return true;
    //     }

    //     return false;
    // }
}
