using UnityEngine;
using TMPro;
using Fusion;

public class AmmoUI : MonoBehaviour
{
    private RaycastWeapon weapon;
    public TextMeshProUGUI ammoTMP;

    void Start()
    {
        FindWeapon();
    }

    void Update()
    {
        if (weapon == null) FindWeapon();

        if (weapon != null && ammoTMP != null)
        {
            ammoTMP.text = $"Đạn: {weapon.GetAmmoCount()}";
        }
    }

    private void FindWeapon()
    {
        var player = FindObjectOfType<NetworkObject>();
        if (player != null)
        {
            weapon = player.GetComponentInChildren<RaycastWeapon>();
        }
    }
}
