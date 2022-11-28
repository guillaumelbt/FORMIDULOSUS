using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask mask;

    private float elipsedTime = 0;
    
    private float speed = 10;
    private Vector3 velocity ;
    private Vector3 targetVelocity;
    
    private bool isGrounded;
    private float groundDistance = 0.5f;
    
    void Awake()
    {
        speed = playerData.speed;
        input.actions["Movement"].started += ctx =>
        {
            elipsedTime = Time.time;
        };
        input.actions["Movement"].canceled += ctx =>
        {
            elipsedTime = Time.time;
            targetVelocity = Vector3.zero;   
        };
        input.actions["Run"].started += ctx =>
        {
            speed = playerData.runSpeed;
        };
        input.actions["Run"].canceled += ctx =>
        {
            speed = playerData.speed;
        };
        input.actions["Jump"].started += ctx =>
        {
            if(isGrounded) Jump();
        };
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance,mask);
        if (input.actions["Movement"].IsPressed()) Move();
        velocity = Vector3.Lerp(velocity, targetVelocity, Time.time - elipsedTime);
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    void Move()
    {
        var axis = input.actions["Movement"].ReadValue<Vector2>().normalized;
        var direction = transform.forward * axis.y + transform.right * axis.x;
        targetVelocity = direction * (speed * Time.deltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * playerData.jumpStrength,ForceMode.Impulse);
    }


}
