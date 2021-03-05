using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CubeController : MonoBehaviour
{
    [SerializeField] Transform cam = null;
    [SerializeField] private float velocity = 20f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float rotationSpeed = 0.05f;
    [SerializeField] private bool blockRotationX = true;
    [SerializeField] private bool blockRotationY = false;
    [SerializeField] private bool blockRotationZ = true;
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private float checkGroundHeight = 1.05f;
    [SerializeField] private float jumpForce = 10000f;
    [SerializeField] private float jumpCooldown = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float applyGravitySpeed = 0.01f;
    [SerializeField] private float dashForce = 100f;
    [SerializeField] private float dashTime = 0.8f;
    [SerializeField] private float dashCooldown = 1.2f;
    [SerializeField] private float forceMultiplier = 1000f;
    [SerializeField] private float clampVelocitySpeed = 0.1f;
    private bool canMove = true;
    private bool canJump = true;
    private bool canDash = true;
    private bool isDashing = false;
    private Rigidbody rb = null;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        BlockRotation();
        ClampSpeed();
        ApplyGravity();
    }

    private void BlockRotation()
    {
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.x = blockRotationX ? 0 : newRotation.x;
        newRotation.y = blockRotationY ? 0 : newRotation.y;
        newRotation.z = blockRotationZ ? 0 : newRotation.z;
        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void ClampSpeed()
    {
        if(isDashing) return;

        Vector3 desiredVelocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        desiredVelocity = Vector3.Lerp(rb.velocity, desiredVelocity, clampVelocitySpeed);
        rb.velocity = desiredVelocity;
    }

    private void ApplyGravity()
    {
        if (IsGrounded()) return;

        Vector3 velocity = rb.velocity;
        velocity.y = Mathf.Lerp(velocity.y, gravity, applyGravitySpeed);
        rb.velocity = velocity;
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        if (!canMove || !IsGrounded()) return;

        Vector2 contextValue = _context.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(contextValue.x, 0, contextValue.y);

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDir.z + right * moveDir.x;

        Rotate(desiredMoveDirection);
        Move(moveDir.magnitude);
    }

    public void OnJump(InputAction.CallbackContext _context)
    {
        if (!canJump || !IsGrounded()) return;

        rb.AddForce(transform.up * jumpForce * forceMultiplier, ForceMode.Force);
        StartCoroutine(JumpCooldown());
    }

    public void OnDash(InputAction.CallbackContext _context)
    {
        if (!canDash) return;

        Debug.Log("Dash");
        rb.AddForce(transform.forward * dashForce * forceMultiplier, ForceMode.Force);
        StartCoroutine(DashCooldown());
    }

    public void Move(float _magnitude)
    {
        Vector3 forcePosition = transform.position + (Vector3.down * 0.5f) + (Vector3.back * 0.5f);
        Vector3 force = transform.forward * _magnitude * velocity;
        rb.AddForce(force, ForceMode.Acceleration);

    }

    public void Rotate(Vector3 _dir)
    {
        Vector3 direction = transform.position + _dir.normalized;
        Vector3 actualDirection = transform.position + transform.forward;
        Vector3 newDirection = Vector3.Lerp(actualDirection, direction, rotationSpeed);
        transform.LookAt(newDirection);
    }

    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit _hit, checkGroundHeight))
        {
            if (_hit.collider.CompareTag(groundTag))
                return true;
        }
        return false;
    }

    IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    IEnumerator DashCooldown()
    {
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown - dashTime);
        canDash = true;
    }
}
