using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private float moveVelocity = 3f;
    
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    private Vector2 moveDirection;
    private bool isWalking;
    private int isWalkingAnimationHash;
    
    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        #endregion
        playerTransform = GetComponent<Transform>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        isWalkingAnimationHash = Animator.StringToHash("isWalking");
    }
    
    private void Update()
    {
        MovePlayer();
        AnimatePlayer();
    }
    
    private void MovePlayer()
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

    private void AnimatePlayer()
    {
        if (isWalking && !playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, true);
        }
        else if (!isWalking && playerAnimator.GetBool(isWalkingAnimationHash))
        {
            playerAnimator.SetBool(isWalkingAnimationHash, false);
        }
    }

}
