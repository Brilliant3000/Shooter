using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireCoolDown;
    public int ammoMagazine;
    public int maxAmmoMagazine;
    public int ammo;
    public int maxAmmo;
    public float range;
    public bool shootOneClick;

    [Header("Animation")]
    public float kickBackForce;
    public float kickBackSmooth;
    public float aimSmooth;
    [Space]

    public Vector3 weaponAimPos;
    [Space]
    public Quaternion weaponAimRot;
    [Space]    
    [Space]   
    [Space]

    [Header("Sounds")]
    public float dropSoundCoolDown;
    [SerializeField] protected AudioClip shotSound, noneAmmo, fall;
    protected AudioSource audioSource;
    

    public ParticleSystem shootPart;
    public GameObject hitPart;
    public WeaponUIController weaponUi;

    [SerializeField] private int weaponLayer;
    [SerializeField] private int weaponLayerGfx;
    [SerializeField] private GameObject[] weaponMash;
    [SerializeField] private Collider[] weaponColliders;

    private GameObject _startPosition;
    private bool _readyToPlaySound = true;
    protected bool _readyToShoot = true;
    private bool _aiming;
    protected bool _reload;
    protected Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }
    public void Update()
    {
        if (_startPosition != null && !_aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition.transform.localPosition, kickBackSmooth * Time.deltaTime);
        }
        if (_aiming)
        {
            transform.localRotation = Quaternion.Inverse(weaponAimRot);
            transform.localPosition = Vector3.Lerp(transform.localPosition, weaponAimPos, aimSmooth * Time.deltaTime);
        }
        if (_startPosition != null && !_aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPosition.transform.localPosition, aimSmooth * Time.deltaTime);
        }
    }
    public virtual void Shoot()
    {
        if (ammoMagazine > 0 && _readyToShoot && !_reload)
        {
            _readyToShoot = false;

            audioSource.PlayOneShot(shotSound);

            transform.localPosition -= new Vector3(0f, 0f, kickBackForce * Time.deltaTime);

            Invoke(nameof(ReadyToShoot), fireCoolDown);

            shootPart.Play();
            ammoMagazine--;

            weaponUi.UpdateAmmoUi(ammoMagazine: ammoMagazine, ammo: ammo);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
            {
                if (hit.collider != null)
                {
                    GameObject holle = Instantiate(hitPart, hit.point, Quaternion.LookRotation(hit.normal));
                    holle.transform.SetParent(hit.transform);
                    if (hit.rigidbody)
                        hit.rigidbody.AddForce(hit.normal * -10, ForceMode.Impulse);
                }
            }
        }
    }

    public virtual void StartReload()
    {
        if (!_reload && ammoMagazine < maxAmmoMagazine && ammo > 0)
        {
            _readyToShoot = false;
            _reload = true;
            Aiming(false);
        }
    }
    public void Aiming(bool activity)
    {
        if (activity && !_reload)
        {
            _aiming = true;
            weaponUi.SetActiveSightAim(false);
        }
        else if (!activity && !_reload)
        {
            _aiming = false;
            weaponUi.SetActiveSightAim(true);
        }
    }
    public virtual void PickUp(GameObject weaponPos)
    {
        foreach (var col in weaponColliders)
            col.enabled = false;
        
        foreach (var gfx in weaponMash)
            gfx.layer = weaponLayerGfx;

        weaponUi.ammoCountUi.SetActive(true);
        weaponUi.UpdateAmmoUi(ammoMagazine: ammoMagazine, ammo: ammo);
        _startPosition = weaponPos;
    }
    public virtual void Drop()
    {
        foreach (var col in weaponColliders)
            col.enabled = true;

        foreach (var gfx in weaponMash)
            gfx.layer = weaponLayer;

        weaponUi.ammoCountUi.SetActive(false);
        audioSource.Stop();
        transform.parent = null;
        _startPosition = null;
    }
    protected void ReadyToShoot()
    {
        _readyToShoot = true;
    }
    public virtual void EndReload()
    {
        int countAmmo = maxAmmoMagazine - ammoMagazine;

        if (ammo > 0 && ammo >= countAmmo)
        {
            ammoMagazine += countAmmo;
        }
        else if (ammo >= 0 && ammo < countAmmo)
        {
            ammoMagazine += ammo;
            ammo = 0;
        }
        if (ammo >= countAmmo)
        {
            ammo -= countAmmo;
        }

        weaponUi.UpdateAmmoUi(ammoMagazine: ammoMagazine, ammo: ammo);
        ReadyToShoot();

        _reload = false;
      
    }

    private void ResetSound()
    {
        _readyToPlaySound = true;
    }
    private void OnCollisionEnter()
    {
        if (_readyToPlaySound)
        {
            audioSource.PlayOneShot(fall);
            Invoke(nameof(ResetSound), dropSoundCoolDown);
            _readyToPlaySound = false;
        }
    }
}
