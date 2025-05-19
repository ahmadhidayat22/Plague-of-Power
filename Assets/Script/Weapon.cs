using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/Create New Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public string weaponName;
    public Sprite weaponSprite;
    public Sprite UIWeaponSprite;
    public GameObject bulletPrefab;
    public float fireRate;
    public int maxAmmo;
    public int totalAmmo;
    public float bulletSpeed;
    public string animationShootName;
    public RuntimeAnimatorController animatorController; // Untuk animasi berbeda tiap senjata
    public bool hasInfinityAmmo;
    public int damage;

}
