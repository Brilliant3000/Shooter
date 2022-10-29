using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireCoolDown;
    public int ammoMagazine;
    public int maxAmmoMagazine;
    public int ammo;
    public int maxAmmo;
    public float timeReload;
    public float range;

    [Header("Animation")]
    public float kickBackForce;
    public float kickBackSmooth;
    public float aimSmooth;
    [Space]

    public Vector3 weaponAimPos;
    [Space]
    public Quaternion weaponAimRot;
    [Space]

    [Header("Sounds")]
    [SerializeField] private AudioClip shotSound, reloadSound, noneAmmo, drop;
    private AudioSource audioSource;

    public ParticleSystem shootPart;
    public GameObject hitPart;
    public WeaponUIController weaponUi;

    [SerializeField] private LayerMask weaponLayer;
    [SerializeField] private LayerMask weaponGfxLayer;
    private Animator anim;
    private GameObject _startPosition;
    private bool _readyToShoot = true;
    private bool _aiming;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    private void Update()
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
    public void Shoot()
    {
        if (ammoMagazine > 0 && _readyToShoot)
        {
            _readyToShoot = false;

            audioSource.PlayOneShot(shotSound);

            transform.localPosition -= new Vector3(0f, 0f, kickBackForce * Time.deltaTime);

            Invoke(nameof(ReadyToShoot), fireCoolDown);

            shootPart.Play();
            ammoMagazine--;

            weaponUi.UpdateAmmoUI(ammoMagazine: ammoMagazine, ammo: ammo);

            RaycastHit hit;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
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

    public void Reload()
    {
        if (_readyToShoot == true && ammoMagazine < maxAmmoMagazine && ammo > 0)
        {
            _readyToShoot = false;

            anim.SetTrigger("Reload");
            Aiming(false);
            audioSource.PlayOneShot(reloadSound);

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

            Invoke(nameof(ReadyToShoot), timeReload);
        }
    }
    private void ReadyToShoot()
    {
        weaponUi.UpdateAmmoUI(ammoMagazine: ammoMagazine, ammo: ammo);
        _readyToShoot = true;
    }
    public void SetWeaponPos(GameObject weaponPos)
    {
        weaponUi.ammoCountUi.SetActive(true);
        weaponUi.UpdateAmmoUI(ammoMagazine: ammoMagazine, ammo: ammo);
        _startPosition = weaponPos;
    }
    public void RemoveWeaponPos()
    {
        weaponUi.UpdateAmmoUI(ammoMagazine: ammoMagazine, ammo: ammo);
        weaponUi.ammoCountUi.SetActive(false);
        audioSource.Stop();
        _startPosition = null;
    }
    public void Aiming(bool activity)
    {
        if (activity)
        {
            _aiming = true;
            weaponUi.SetActiveSightAim(false);
        }
        else if(!activity)
        {
            _aiming = false;
            weaponUi.SetActiveSightAim(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision != null)
        {
            audioSource.PlayOneShot(drop);
        }
    }
}
