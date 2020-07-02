using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;
    public PlayerChange playerController;
    public RuntimeAnimatorController player1Anim;
    public RuntimeAnimatorController player2Anim;

    public Avatar player1Avatar;
    public Avatar player2Avatar;


    public GameObject effect;

    public float cooldownTime = 3f;
    private float nextFireTime = 0f;

    private static bool dpadLeftPressed = false;
    private static bool dpadRightPressed = false;
    private static bool dpadUpPressed = false;
    private static bool dpadDownPressed = false;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    float t = 0;

    public enum Stance_Type
    {
        player1,
        player2,
        TOTAL_STANCE,
    }

    public Stance_Type currStance;
    Animator animator;
    Transform cameraT;
    CharacterController controller;



    void Start()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        effect.SetActive(false);
    }

    void Update()
    {
        // input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") * Time.deltaTime);
        Vector2 inputDir = input.normalized;
        bool running = Input.GetButton("RightBumper");

        float dpadHorizontal = Input.GetAxis("Dpad Horizontal");
        float dpadVertical = Input.GetAxis("Dpad Vertical");

        Move(inputDir, running);

        if (Time.time > nextFireTime)
        {
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
                Jump();
                nextFireTime = Time.time + cooldownTime;
            }

        }



        //Dpad
        if (Input.GetAxis("Dpad Horizontal") < -0.001f && !dpadLeftPressed)  //范
        {
            dpadRightPressed = false;
            dpadLeftPressed = true;
            dpadUpPressed = false;
            dpadDownPressed = false;
            

            effect.SetActive(true);
            

        }
        else if (Input.GetAxis("Dpad Horizontal") > 0.001f && !dpadRightPressed)  //甘
        {
            dpadRightPressed = true;
            dpadLeftPressed = false;
            dpadUpPressed = false;
            dpadDownPressed = false;
            NextStance();

            effect.SetActive(true);
          

        }

        if (Input.GetAxis("Dpad Vertical") < -0.001f && !dpadDownPressed)  //柳
        {
            dpadRightPressed = false;
            dpadLeftPressed = false;
            dpadUpPressed = false;
            dpadDownPressed = true;
            NextStance2();

            effect.SetActive(true);
           
        }
        else if (Input.GetAxis("Dpad Vertical") > 0.001f && !dpadUpPressed)  //謝
        {
            dpadRightPressed = false;
            dpadLeftPressed = false;
            dpadUpPressed = true;
            dpadDownPressed = false;
           

            effect.SetActive(true);
            
        }

        t += Time.deltaTime;



        // animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    private void FixedUpdate()
    {
        PlayerFootsteps();
    }

    private void PlayerFootsteps()
    {
        if (currentSpeed > 0.1f && currentSpeed < walkSpeed + 0.1f)
        {

        }

        if (currentSpeed < 0.1f)
        {

        }
    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ground")
        {
            if (controller.isGrounded)
            {
                velocityY = 0;
            }
            Debug.Log("碰到了");
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {

            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;

        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    public Stance_Type GetStance()
    {
        return currStance;
    }

    public void NextStance()
    {
        currStance = Stance_Type.player1;
        if (currStance == Stance_Type.TOTAL_STANCE)
            currStance = 0;
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;
                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }
    }
    public void NextStance2()
    {
        currStance = Stance_Type.player2;
        if (currStance == Stance_Type.TOTAL_STANCE)
            currStance = 0;
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;

                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }

    }
    
    public void PrevStance()
    {
        if (currStance != 0)
        {
            currStance--;
        }
        else
        {
            currStance = Stance_Type.TOTAL_STANCE - 1;
        }
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;
                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }
    }


}