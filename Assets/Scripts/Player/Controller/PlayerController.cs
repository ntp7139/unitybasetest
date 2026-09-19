using UnityEngine;

public class PlayerController : MonoBehaviour, ICanSendEvent, ICanListenEvent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    Animator animator;

    [SerializeField]
    Rigidbody rigidPlayer;

    [SerializeField]
    GameObject modelPlayer;

    [SerializeField]
    MainCamera mainCamera;

    [SerializeField]
    float speed = 3f;

    [SerializeField]
    int jumpForce = 12000;

    [SerializeField]
    Vector3 prevPos;

    [SerializeField]
    Vector3 prevDirection;

    [SerializeField]
    private Collider playerCollider;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float groundCheckHeight = 0.1f;

    [SerializeField]
    private float groundCheckOffset = 0.02f;
    StateMachine _stateMachine;
    IState _idleState;
    IState _runState;
    IState _jumpState;
    IState _interactState;
    IState _deathState;
    EventSubscription _onExecuteInteract;

    void Start()
    {
        _idleState = new IdleState(this);
        _runState = new RunState(this);
        _jumpState = new JumpState(this);
        _interactState = new InteractState(this);
        _deathState = new DeathState(this);
        _stateMachine = new StateMachine(_idleState);
    }

    void OnEnable()
    {
        _onExecuteInteract = this.Listen<OnExecuteInteractEvent>(OnExecuteInteract);
    }

    void OnDisable()
    {
        _onExecuteInteract?.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine?.Update();
    }

    void FixedUpdate()
    {
        prevDirection = transform.position - prevPos;
        prevPos = transform.position;
    }

    void LateUpdate() { }

    public void OnExecuteInteract(OnExecuteInteractEvent evt)
    {
        ChangeInteractState();
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }

    public Vector3 Move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (direction != Vector3.zero)
        {
            if (mainCamera != null)
            {
                direction = Quaternion.Euler(0, mainCamera.Yaw, 0) * direction;
            }
            transform.position = transform.position + direction.normalized * speed * Time.deltaTime;
            Rotate(direction);
        }
        return direction;
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidPlayer?.AddForce(Vector3.up * jumpForce);
        }
    }

    public bool IsJumpDown()
    {
        bool isJumpDown = false;

        if (prevDirection.y < -0.01f)
        {
            isJumpDown = true;
        }
        return isJumpDown;
    }

    public void Rotate(Vector3 direction)
    {
        if (modelPlayer != null)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetAngle = Quaternion.Euler(0, angle, 0);
            modelPlayer.transform.rotation = Quaternion.Lerp(
                modelPlayer.transform.rotation,
                targetAngle,
                1f
            );
        }
    }

    public void ChangeIdleState()
    {
        _stateMachine?.ChangeState(_idleState);
    }

    public void ChangeRunState()
    {
        _stateMachine?.ChangeState(_runState);
    }

    public void ChangeJumpState()
    {
        _stateMachine?.ChangeState(_jumpState);
    }

    public void ChangeInteractState()
    {
        _stateMachine?.ChangeState(_interactState);
    }

    public void ChangeDeathState()
    {
        _stateMachine?.ChangeState(_deathState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractableObject"))
        {
            ICanInteract interactObject = other.gameObject.GetComponent<ICanInteract>();
            if (interactObject != null)
            {
                this.Publish(new OnEnterInteractEvent(interactObject));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("InteractableObject"))
        {
            ICanInteract interactObject = other.gameObject.GetComponent<ICanInteract>();
            if (interactObject != null)
            {
                this.Publish(new OnExitInteractEvent());
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathGround"))
        {
            ChangeDeathState();
        }
    }

    public void OnDeath()
    {
        this.Publish(new OnDeathEvent());
    }

    public bool IsGrounded()
    {
        Bounds bounds = playerCollider.bounds;

        Vector3 center = new Vector3(
            bounds.center.x,
            bounds.min.y - groundCheckOffset,
            bounds.center.z
        );

        Vector3 halfExtents = new Vector3(
            bounds.extents.x * 1.1f,
            groundCheckHeight,
            bounds.extents.z * 1.1f
        );

        return Physics.CheckBox(center, halfExtents, Quaternion.identity, groundLayer);
    }
}
