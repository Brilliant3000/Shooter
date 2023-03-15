using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speedWalk;
    public float speedRun;
    public float speedMoveAiming;
    public float groundDrag;
    [Space]
    [Header("Crouch")]
    public float speedCrouch;
    public float cameraOffset;
    public float cameraSmooth;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [Space]

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [Space]

    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject cameraPosition;

    private Vector3 startCameraPos;
    private float startSpeedWalk;
    private bool readyToJump = true;
    private bool grounded;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startSpeedWalk = speedWalk;
        startCameraPos = cameraPosition.transform.localPosition;
    }

    private void FixedUpdate()
    {
        Crouch();
        Walk();
        Run();
        SpeedControll();
        ResetMoveSpeed();
    }
    private void Update()
    {
        CheckGround();
        Jump();
    }
    private void Walk()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * moveInputY + orientation.right * moveInputX;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * speedWalk * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speedWalk * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedWalk = speedRun;
        }
    }
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speedWalk = speedCrouch;
            cameraPosition.transform.localPosition = Vector3.Lerp(cameraPosition.transform.localPosition, new Vector3(0f, cameraOffset, 0f), cameraSmooth);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded && readyToJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
            readyToJump = false;
        }
    }

    public void AimingWalkSpeed(bool activity)
    {
        if(activity)
        {
            speedWalk = speedMoveAiming;
        }
        else
        {
            speedWalk = startSpeedWalk;
        }
    }

    private void CheckGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 5 + 0.2f, whatIsGround);
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void SpeedControll()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVelocity.magnitude > speedWalk)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * speedWalk;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void ResetMoveSpeed()
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            speedWalk = startSpeedWalk;
            cameraPosition.transform.localPosition = Vector3.Lerp(cameraPosition.transform.localPosition, startCameraPos, cameraSmooth);
        }
    }
  
    private void ResetJump()
    {
        readyToJump = true;
    }
}
