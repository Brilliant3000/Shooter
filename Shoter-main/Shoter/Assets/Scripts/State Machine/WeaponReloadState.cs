using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloadState : StateMachineBehaviour
{
    private Weapon weapon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(weapon == null)
            weapon = animator.GetComponent<Weapon>();
        weapon.StartReload();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weapon.EndReload();
    }
}
