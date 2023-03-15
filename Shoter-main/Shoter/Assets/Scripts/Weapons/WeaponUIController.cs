using UnityEngine;
using TMPro;

public class WeaponUIController : MonoBehaviour
{
    public GameObject ammoCountUi;
    [SerializeField] private GameObject signPickup;
  
    public GameObject sightAim;
    private TextMeshProUGUI ammoCountUiText;
    private void Start()
    {
        ammoCountUiText = ammoCountUi.GetComponent<TextMeshProUGUI>();
    }
    public void UpdateAmmoUi(int ammoMagazine, int ammo)
    {
        ammoCountUiText.text = $"{ammoMagazine} / {ammo}";
    }
    public void SetActiveSightAim(bool activeMode)
    {
        sightAim.SetActive(activeMode);
    }
    public void SetActiveSignPickup(bool activeMode)
    {
            signPickup.SetActive(activeMode);
    }
}
