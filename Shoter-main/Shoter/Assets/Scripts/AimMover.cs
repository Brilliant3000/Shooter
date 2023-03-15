using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimMover : MonoBehaviour
{
    public float speed;
    public float moveSpeedY;
    public bool activity;
    public Vector3 direction = Vector3.forward;
    public LayerMask layer;

    private Vector3 _startPosition;

    public bool moveDown;
    public bool moveUp;
    public bool readyToShooting;

    public AimRepeater aimRepeater;

    private void Start()
    {
        readyToShooting = true;
        _startPosition = transform.position;    
    }

    void Update()
    {
        CheckWall();
    }

    private void FixedUpdate()
    {
        Move();
    
        if (moveDown)
            HideAim();
        if (moveUp)
            RepeatAim();
    }

    private void Move()
    {
        if (activity)
            transform.Translate(direction * speed * Time.deltaTime);
    }
    private void CheckWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f, layer))
        {
            direction = Vector3.back;
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1f, layer))
        {
            direction = Vector3.forward;
        }
    }

    public void HitAim()
    {
        if (readyToShooting)
        {
            activity = false;
            aimRepeater.UpdateValues(this);
        }
    }

    private void HideAim()
    {
        transform.Translate(Vector3.down * moveSpeedY * Time.fixedDeltaTime);
        if (transform.position.y < -2)
            moveDown = false;
    }

    public void RepeatAim()
    {
        transform.Translate(Vector3.up * moveSpeedY * Time.fixedDeltaTime);
        if (transform.position.y >= _startPosition.y)
            moveUp = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.back);
    }
}