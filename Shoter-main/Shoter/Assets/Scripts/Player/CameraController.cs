using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orientation;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoveCamera();
    }
    private void MoveCamera()
    {
        float mousePositionX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mousePositionY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        rotationY += mousePositionX;
        rotationX -= mousePositionY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);


    }
}
