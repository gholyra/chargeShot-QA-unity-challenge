using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private float moveVelocity = 3f;
    [SerializeField] private float jumpForce = 3f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundPositionChecker;
    
    [SerializeField] private Transform shotAnchor;
    [SerializeField] private Transform shotPrefab;
    
    [SerializeField, Range(1f, 10f)] private float jumpFallGravityMultiplier = 3f;
    
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    private Vector2 moveDirection;
    private float initialGravityScale;
    
    private bool isWalking;
    private bool isJumping;
    private bool isStandShooting;
    private bool canJump;

    private int isWalkingAnimationHash;
    private int isJumpingAnimationHash;
    private int isStandShootingAnimationHash;

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        #endregion

        GetPlayerComponents();
        GetAnimatorParametersHash();
        initialGravityScale = playerRigidBody.gravityScale;
        inputManager.OnJumpAction += HandleJumpAction;
        inputManager.OnShootAction += HandleShootAction;
    }

    private void Update()
    {
        canJump = IsGrounded();
        HandleGravity();
        HandleMovement();
        HandleAnimation();
    }

    #region Getters
    private void GetPlayerComponents()
    {
        playerTransform = GetComponent<Transform>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void GetAnimatorParametersHash()
    {
        isWalkingAnimationHash = Animator.StringToHash("isWalking");
        isJumpingAnimationHash = Animator.StringToHash("isJumping");
        isStandShootingAnimationHash = Animator.StringToHash("isStandShooting");
    }
    #endregion

    #region Handlers
    private void HandleJumpAction(object sender, InputManager.OnHoldInteractionData e)
    {
        e.gameControls.Player.Jump.started += context =>
        {
            if (canJump && context.interaction is TapInteraction)
            {
                playerRigidBody.velocity += Vector2.up * jumpForce;
                Debug.Log("pulei pouco");
            }
        };

        e.gameControls.Player.Jump.performed += context =>
        {
            if (canJump && context.interaction is HoldInteraction)
            {
                Debug.Log("performou muito");
                playerRigidBody.velocity += Vector2.up * (jumpForce * 1.5f);
            }
        };

        e.gameControls.Player.Jump.canceled += context =>
        {
            if (context.interaction is TapInteraction)
            {
                Debug.Log("cancelou pouco");
            }
            else if (context.interaction is HoldInteraction)
            {
                Debug.Log("cancelou muito");
            }
        };
    }
    
    private void HandleShootAction(object sender, InputManager.OnHoldInteractionData e)
    {
        e.gameControls.Player.Shoot.started += context =>
        {
            isStandShooting = true;
            Instantiate(shotPrefab, shotAnchor.position, shotAnchor.rotation);
        };
        
        e.gameControls.Player.Shoot.performed += context =>
        {
            isStandShooting = false;
        };
    }
    
    private void HandleGravity()
    {
        if (canJump)
        {
            isJumping = false;
            playerRigidBody.gravityScale = initialGravityScale;
        }
        else if (isJumping && playerRigidBody.velocity.y < 0f)
        {
            playerRigidBody.gravityScale = initialGravityScale * jumpFallGravityMultiplier;
        }
        else
        {
            isJumping = true;
        }
    }
    
    private void HandleMovement()
    {
        moveDirection.x = inputManager.GetMovementDirection();

        isWalking = moveDirection.x != 0;

        if (moveDirection.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            playerSprite.flipX = true;
        }

        playerTransform.Translate(moveDirection * moveVelocity * Time.deltaTime);
    }
    
    private void HandleAnimation()
    {
        #region Walking Parameter

        if (isWalking && !playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, true);
        }
        else if (!isWalking && playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, false);
        }

        #endregion

        #region Jumping Parameter
        if (isJumping && !playerAnimator.GetBool(isJumpingAnimationHash))
        {
            playerAnimator.SetBool(isJumpingAnimationHash, true);
        }
        else if (!isJumping && playerAnimator.GetBool(isJumpingAnimationHash))
        {
            playerAnimator.SetBool(isJumpingAnimationHash, false);
        }
        #endregion
        
        #region Stand Shooting Parameter
        if (isStandShooting && !playerAnimator.GetBool(isStandShootingAnimationHash))
        {
            playerAnimator.SetBool(isStandShootingAnimationHash, true);
        }
        else if (!isStandShooting && playerAnimator.GetBool(isStandShootingAnimationHash))
        {
            playerAnimator.SetBool(isStandShootingAnimationHash, false);
        }
        #endregion
    }
    #endregion

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.OverlapBox(groundPositionChecker.position, new Vector2(1f, .2f), groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundPositionChecker.position, new Vector2(1f, .2f));
    }

}