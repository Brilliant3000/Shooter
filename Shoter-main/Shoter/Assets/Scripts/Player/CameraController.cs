using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;
    [SerializeField] private Transform orientation;

    private float rotationX;
    private float rotationY;

    void Start()
    {

    }

    void Update()
    {
        MoveCamera();
    }
    private void MoveCamera()
    {
        float mousePositionX = Input.GetAxisRaw("Mouse X") * sensX;
        float mousePositionY = Input.GetAxisRaw("Mouse Y") * sensY;

        rotationY += mousePositionX * Time.deltaTime;
        rotationX -= mousePositionY * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
