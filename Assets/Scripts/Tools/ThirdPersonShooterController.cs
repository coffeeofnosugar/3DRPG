using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    Transform playerTransform;
    Animator animator;
    Transform cameraTransform;
    CharacterController characterController;
    PlayerSoundController playerSoundController;  //��ɫ��Ч���Žű�

    #region �����̬����ض���������ֵ
    public enum PlayerPosture
    {
        Crouch,
        Stand,
        Falling,
        Jumping,
        Landing
    };
    [HideInInspector]
    public PlayerPosture playerPosture = PlayerPosture.Stand;

    float crouchThreshold = 0f;
    float standThreshold = 1f;
    float midairThreshold = 2.1f;
    float landingThreshold = 1f;
    #endregion

    //����˶�״̬
    public enum LocomotionState
    {
        Idle,
        Walk,
        Run
    };
    [HideInInspector]
    public LocomotionState locomotionState = LocomotionState.Idle;


    //���װ��״̬
    public enum ArmState
    {
        Normal,
        Aim
    };
    [HideInInspector]
    public ArmState armState = ArmState.Normal;

    //��Ҳ�ͬ״̬���˶��ٶ�
    float crouchSpeed = 1.5f;
    float walkSpeed = 2.5f;
    float runSpeed = 5.5f;

    #region ����ֵ
    Vector2 moveInput;
    bool isRunning;
    bool isCrouch;
    bool isAiming;
    bool isJumping;
    #endregion

    #region ״̬�������Ĺ�ϣֵ
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int verticalVelHash;
    int feetTweenHash;
    #endregion


    Vector3 playerMovement = Vector3.zero;

    //����
    public float gravity = -9.8f;

    //��ֱ�����ٶ�
    float VerticalVelocity;

    //�������߶�
    public float maxHeight = 1.5f;

    //�Ϳ����ҽ�״̬
    float feetTween;

    #region �ٶȻ���ض���
    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentChacheIndex = 0;
    Vector3 averageVel = Vector3.zero;
    #endregion

    //����ʱ���ٶȵı���
    float fallMultiplier = 1.5f;

    //����Ƿ��ŵ�
    bool isGrounded;

    //����Ƿ���Ե���
    bool couldFall;

    //�������С�߶ȣ�С�ڴ˸߶Ȳ����л���������̬
    float fallHeight = 0.5f;

    //�Ƿ�����ԾCD״̬
    bool isLanding;

    //�ر������ߵ�ƫ����
    float groundCheckOffset = 0.5f;

    //��Ծ��CD����
    float jumpCD = 0.15f;

    //��һ֡�Ķ���nornalizedʱ��
    float lastFootCycle = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        playerSoundController = GetComponent<PlayerSoundController>();

        postureHash = Animator.StringToHash("�����̬");
        moveSpeedHash = Animator.StringToHash("�ƶ��ٶ�");
        turnSpeedHash = Animator.StringToHash("ת���ٶ�");
        verticalVelHash = Animator.StringToHash("��ֱ�ٶ�");
        feetTweenHash = Animator.StringToHash("���ҽ�");

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        SwitchPlayerStates();
        CaculateGravity();
        Jump();
        CaculateInputDirection();
        SetupAnimator();
        PlayFootStep();
    }

    #region �������
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void GetRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValueAsButton();
    }

    public void GetCrouchInput(InputAction.CallbackContext ctx)
    {
        isCrouch = ctx.ReadValueAsButton();
    }

    public void GetAimInput(InputAction.CallbackContext ctx)
    {
        isAiming = ctx.ReadValueAsButton();
    }

    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        isJumping = ctx.ReadValueAsButton();
    }

    #endregion


    /// <summary>
    /// �����л���ҵĸ���״̬
    /// </summary>
    void SwitchPlayerStates()
    {
        if (!isGrounded)
        {
            if(VerticalVelocity > 0)
            {
                playerPosture = PlayerPosture.Jumping;
            }
            else if(playerPosture != PlayerPosture.Jumping)
            {
                if(couldFall)
                {
                    playerPosture = PlayerPosture.Falling;
                }
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            StartCoroutine(CoolDownJump());
        }
        else if (isLanding)
        {
            playerPosture = PlayerPosture.Landing;
        }
        else if (isCrouch)
        {
            playerPosture = PlayerPosture.Crouch;
        }
        else
        {
            playerPosture = PlayerPosture.Stand;
        }

        if (moveInput.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else if (!isRunning)
        {
            locomotionState = LocomotionState.Walk;
        }
        else
        {
            locomotionState = LocomotionState.Run;
        }

        if (isAiming)
        {
            armState = ArmState.Aim;
        }
        else
        {
            armState = ArmState.Normal;
        }
    }


    /// <summary>
    /// ��ؼ��
    /// </summary>
    void CheckGround()
    {
        if (Physics.SphereCast(playerTransform.position + (Vector3.up * groundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - characterController.radius + 2 * characterController.skinWidth))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            couldFall = !Physics.Raycast(playerTransform.position, Vector3.down, fallHeight);
        }
    }

    /// <summary>
    /// ������ԾCD
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolDownJump()
    {
        landingThreshold = Mathf.Clamp(VerticalVelocity, -10, 0);
        landingThreshold /= 20f;
        landingThreshold += 1f;
        isLanding = true;
        playerPosture = PlayerPosture.Landing;
        playerSoundController.PlayLanding();
        yield return new WaitForSeconds(jumpCD);
        isLanding = false;
    }

    /// <summary>
    /// ���������ٶ�
    /// </summary>
    void CaculateGravity()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if(!isGrounded)
            {
                VerticalVelocity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                VerticalVelocity = gravity * Time.deltaTime;
            }
        }
        else
        {
            if (VerticalVelocity <= 0 || !isJumping)
            {
                VerticalVelocity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                VerticalVelocity += gravity * Time.deltaTime;
            }
        }

    }

    /// <summary>
    /// ��Ծ����
    /// </summary>
    void Jump()
    {
        if (playerPosture == PlayerPosture.Stand && isJumping)
        {
            playerSoundController.PlayJumpEffort();
            VerticalVelocity = Mathf.Sqrt(-2 * gravity * maxHeight);
            feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
            feetTween = feetTween < 0.5f ? 1 : -1;
            if (locomotionState == LocomotionState.Run)
            {
                feetTween *= 3;
            }
            else if (locomotionState == LocomotionState.Walk)
            {
                feetTween *= 2;
            }
            else
            {
                feetTween = Random.Range(0.5f, 1f) * feetTween;
            }
        }
    }


    /// <summary>
    /// ��������������������ķ���
    /// </summary>
    void CaculateInputDirection()
    {
        Vector3 camForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        playerMovement = camForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        playerMovement = playerTransform.InverseTransformVector(playerMovement);
    }


    /// <summary>
    /// ���ö���״̬���Ĳ���
    /// </summary>
    void SetupAnimator()
    {
        if (playerPosture == PlayerPosture.Stand)
        {
            animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Crouch)
        {
            animator.SetFloat(postureHash, crouchThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                default:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * crouchSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelHash, VerticalVelocity);
            animator.SetFloat(feetTweenHash, feetTween);
        }
        else if (playerPosture == PlayerPosture.Landing)
        {
            animator.SetFloat(postureHash, landingThreshold, 0.03f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:
                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if(playerPosture == PlayerPosture.Falling)
        {
            animator.SetFloat(postureHash, midairThreshold);
            animator.SetFloat(verticalVelHash, VerticalVelocity);
        }

        if (armState == ArmState.Normal && playerPosture != PlayerPosture.Jumping)
        {
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            animator.SetFloat(turnSpeedHash, rad, 0.1f, Time.deltaTime);
            playerTransform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
    }


    /// <summary>
    /// ����ǰ��֡���ٶ�ƽ��ֵ
    /// </summary>
    /// <param name="newVel">��ǰ֡���ٶ�ƽ��ֵ</param>
    /// <returns>ƽ���ٶ�</returns>
    Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }

    /// <summary>
    /// ���ŽŲ���
    /// </summary>
    void PlayFootStep()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if (locomotionState == LocomotionState.Walk || locomotionState == LocomotionState.Run)
            {
                float currentFootCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f); ;
                if ((lastFootCycle < 0.1 && currentFootCycle >= 0.1) || (currentFootCycle >= 0.6 && lastFootCycle < 0.6))
                {
                    playerSoundController.PlayFootStep();
                }
                lastFootCycle = currentFootCycle;
            }
        }
    }

    /// <summary>
    /// ����ϵͳ�Ļص�����
    /// </summary>
    private void OnAnimatorMove()
    {

        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            Vector3 playerDeltaMovement = animator.deltaPosition;
            playerDeltaMovement.y = VerticalVelocity * Time.deltaTime;
            characterController.Move(playerDeltaMovement);
            averageVel = AverageVel(animator.velocity);
        }
        else
        {
            averageVel.y = VerticalVelocity;
            Vector3 playerDeltaMovement = averageVel * Time.deltaTime;
            characterController.Move(playerDeltaMovement);
        }
    }
}