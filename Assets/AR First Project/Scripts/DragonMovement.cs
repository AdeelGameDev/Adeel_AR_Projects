using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;

    private FixedJoystick joystick;
    private const string IS_Moving = "IsMoving";

    private Animator animator;
    private Transform cameraTransform;

    private void Awake()
    {
        joystick = GameObject.FindObjectOfType<FixedJoystick>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        if (moveDirection != Vector3.zero)
        {
            // Adjust moveDirection to be relative to the camera's orientation
            Vector3 adjustedMoveDirection = cameraTransform.TransformDirection(moveDirection);
            adjustedMoveDirection.y = 0; // Keep the movement on the XZ plane

            Quaternion toRotation = Quaternion.LookRotation(adjustedMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, moveSpeed * Time.deltaTime);

            animator.SetBool(IS_Moving, true);
            transform.Translate(adjustedMoveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            animator.SetBool(IS_Moving, false);
        }
    }
}
