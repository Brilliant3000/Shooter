using UnityEngine;
using TMPro;

public class WeaponUIController : MonoBehaviour
{
    public GameObject ammoCountUi;
  
    public GameObject sightAim;
    private TextMeshProUGUI ammoCountUiText;
    private void Start()
    {
        ammoCountUiText = ammoCountUi.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateAmmoUI(int ammoMagazine, int ammo)
    {
        ammoCountUiText.text = $"{ammoMagazine} / {ammo}";
    }
    public void SetActiveSightAim(bool activeMode)
    {
        sightAim.SetActive(activeMode);
    }
}
