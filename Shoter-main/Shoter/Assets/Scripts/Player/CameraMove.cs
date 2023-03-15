using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject cameraPos;

    void Update()
    {
        transform.position = cameraPos.transform.position;
    }
}
