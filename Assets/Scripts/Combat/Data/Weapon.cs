using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData weaponData;

    [Header("References")]
    [SerializeField] private Transform weaponRoot;
    [SerializeField] private Transform vfxSpawnPoint;
    [SerializeField] private Transform hitboxRoot;

    public WeaponData WeaponData => weaponData;

    public Transform WeaponRoot => weaponRoot;
    public Transform VFXSpawnPoint => vfxSpawnPoint;
    public Transform HitboxRoot => hitboxRoot;

    public void Equip()
    {
        gameObject.SetActive(true);
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
    }
}