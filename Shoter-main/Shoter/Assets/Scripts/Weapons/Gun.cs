using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public AudioClip[] sound;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.speed = reloadSpeed;
    }
    public override void StartReload()
    {
        if (!_reload && ammoMagazine < maxAmmoMagazine && ammo > 0)
        {
            anim.StopPlayback();
            anim.SetTrigger("Reload");
            base.StartReload();
        }
    }

    public override void Drop()
    {
        anim.Play(0);
        anim.StartPlayback();
        _readyToShoot = true;
        _reload = false;
        base.Drop();
    }

    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(sound[index]);
    }
}
