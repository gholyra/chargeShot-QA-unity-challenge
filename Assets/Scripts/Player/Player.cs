using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private float moveVelocity = 3f;
    [SerializeField] private float jumpForce = 3f;
    
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    private Vector2 moveDirection;
    private bool isWalking;
    private bool isJumping;
    private bool canJump;
    
    private int isWalkingAnimationHash;
    private int isJumpingAnimationHash;
    
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
        inputManager.OnJumpAction += HandleJumpAction;
    }

    private void Update()
    {
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
    }
    #endregion
    
    #region Handlers
    private void HandleJumpAction(object sender, EventArgs e)
    {
        playerRigidBody.AddForce(Vector2.up * jumpForce);
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
        if (isJumping && !playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, true);
        }
        else if (!isWalking && playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, false);
        }
        #endregion
    }
    #endregion

}
