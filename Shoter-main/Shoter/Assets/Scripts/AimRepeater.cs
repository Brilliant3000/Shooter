using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class AimRepeater : MonoBehaviour
{
    [SerializeField] private AimMover[] _aimMovers;

    private void Start()
    {
        for (int i = 0; i < _aimMovers.Length; i++)
        {
            _aimMovers[i].aimRepeater = this;
        }
    }
    public void UpdateValues(AimMover aim)
    {
        CheckAimsActivity();
    }

    private void CheckAimsActivity()
    {
        for (int i = 0; i < _aimMovers.Length; i++)
        {
            if (_aimMovers.ElementAt(i).activity == true) return; 
        }
        StartCoroutine(StayTime());
    }

    private IEnumerator StayTime()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _aimMovers.Length; i++)
        {
            _aimMovers[i].moveDown = true;
            _aimMovers[i].readyToShooting = false;
        }  
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _aimMovers.Length; i++)
        {
            _aimMovers[i].moveUp = true;
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _aimMovers.Length; i++)
        {
            _aimMovers[i].activity = true;
            _aimMovers[i].speed++; 
            Mathf.Clamp(_aimMovers[i].speed,0, 22);
            _aimMovers[i].readyToShooting = true;
        }
    }
}
