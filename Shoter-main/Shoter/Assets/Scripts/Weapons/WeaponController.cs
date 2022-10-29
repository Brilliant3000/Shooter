using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float pickUpDistance;
    public float dropForce;
    public float zoomSmooth = 11;
    public float zoomSize = 40f;
    public float startZoom = 60f;

    public Weapon weapon;
    public GameObject weaponPos;
    public LayerMask layerWeapon;

    [Header("Sounds")]
    public AudioClip pickup;

    private PlayerMovement _playerMovement;
    private AudioSource _audioSoursce;
    private Camera _camera;
    private bool aiming;

    private void Start()
    {
        _camera = Camera.main;
        _audioSoursce = GetComponent<AudioSource>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && weapon != null)
        {
            Shoot();
        }
        if(Input.GetKey(KeyCode.Mouse1) && weapon != null)
        {
            Aiming(true);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1) && weapon != null)
        {
            Aiming(false);
        }
        if (Input.GetKeyDown(KeyCode.R) && weapon != null)
        {
            Reload();
        }
        if(Input.GetKeyDown(KeyCode.E) && weapon == null)
        {
            PickUp();
        }
        if(Input.GetKey(KeyCode.Q) && weapon != null)
        {
            Drop();
        }
        CameraZoom();
    }   
    private void Shoot()
    {
        weapon.Shoot();
    }
    private void Reload()
    {
        weapon.Reload();
        aiming = false;
    }
    private void PickUp()
    {
        RaycastHit hit;
        if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, pickUpDistance, layerWeapon))
        {
            if(hit.collider.gameObject != null)
            {
                weapon = hit.collider.GetComponent<Weapon>();
                weapon.SetWeaponPos(weaponPos);
                hit.rigidbody.isKinematic = true;
                hit.collider.enabled = false;
                hit.collider.gameObject.transform.SetParent(weaponPos.transform);
                hit.collider.transform.rotation = weaponPos.transform.rotation;
                hit.collider.gameObject.transform.position = weaponPos.transform.position;
                _audioSoursce.PlayOneShot(pickup);
            }
        }
    }
    private void Drop()
    {
        weapon.RemoveWeaponPos();
        Collider weaponCol = weapon.GetComponentInParent<Collider>();
        Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
        weaponRb.isKinematic = false;
        weaponCol.enabled = true;
        weaponRb.AddForce(_camera.transform.forward * dropForce, ForceMode.Impulse);
        weaponCol.transform.parent = null;
        weapon.Aiming(false);
        weapon = null;
    }
    private void Aiming(bool activity)
    {
        if (activity)
        {
            aiming = true;
            _playerMovement.AimingWalkSpeed(true);
            weapon.Aiming(true);
        }
        else if (!activity)
        {
            aiming = false;
            _playerMovement.AimingWalkSpeed(false);
            weapon.Aiming(false);
        }
    }
    private void CameraZoom()
    {
        if(aiming && _camera.fieldOfView > zoomSize)
        {
            _camera.fieldOfView -= zoomSmooth * Time.deltaTime;
        }
        else if(!aiming && _camera.fieldOfView < startZoom)
        {
            _camera.fieldOfView += zoomSmooth * Time.deltaTime;
        }
    }
}
