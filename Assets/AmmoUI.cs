using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public RaycastWeapon weapon; // Tham chiếu đến vũ khí
    public Text ammoText; // Nếu dùng UI Text
    public TextMeshProUGUI ammoTMP; // Nếu dùng TextMeshPro

    void Update()
    {
        if (weapon != null)
        {
            int ammoCount = weapon.GetAmmoCount(); // Lấy số lượng đạn hiện tại

            if (ammoText != null)
                ammoText.text = "Đạn: " + ammoCount.ToString();

            if (ammoTMP != null)
                ammoTMP.text = "Đạn: " + ammoCount.ToString();
        }
    }
}
