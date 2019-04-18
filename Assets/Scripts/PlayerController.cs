using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Movement")]
    private Vector3 moveDirection;
    public bool isMoving;
    public bool canMove;
    public bool canJump;
    public float maxMoveSpeed;
    float moveSpeedX;
    float moveSpeedZ;
    Vector3 velocity;

    [Header("Camera")]
    public GameObject camPivot;
    GameObject playerCam;
    Transform playerTrans;
    Transform pivTrans;
    Rigidbody myRigidbody;
    PlayerUIController uiController;
    public float playerRotSpeed;

    [Header("Raycast Shooting")]
    public float shootingDistance;
    public float weaponDamage;
    public float critMultiplier;
    public LayerMask enemyLayer;

    [Header("Jumping")]
    Vector2 movementInput = new Vector2();
    public bool grounded;
    bool jumpInput;
    public float checkGroundDown;
    public float jumpForce = 1.3f;
    public int maxJumpCount = 2;
    public int minJumpCount = 0;
    public int jumpCountIntervals = 1;
    public int currentJumpCount;
    public ParticleSystem butterflyJump;

    [Header("Followers")]
    public int followerCount;
    

    //EnemyController enemyCont;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;        
        canMove = true;
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        uiController = GetComponentInChildren<PlayerUIController>(); 

        playerTrans = this.gameObject.transform;
        pivTrans = camPivot.transform;
        myRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //enemyCont = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>(); 
        checkGroundDown = (GetComponent<CapsuleCollider>().height / 2) + 0.01f;
        jumpInput = jumpInput || Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        Movement();
        Shooting();
        CheckGrounded();
        CheckJump();
        FaceCamera();
    }

    public void Movement()
    {
        if (canMove)
        { 
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * moveZ * maxMoveSpeed;
            gameObject.GetComponent<Rigidbody>().velocity += transform.right * moveX * maxMoveSpeed;
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
            {
                isMoving = true;
                
            }
            else
                isMoving = false;

        }  
    }

     void FaceCamera()
    {
        if (isMoving)
        {
            Quaternion lookPos = camPivot.transform.rotation;
            lookPos.x = transform.rotation.x; lookPos.y = camPivot.transform.rotation.y; lookPos.z = transform.rotation.z;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookPos, Time.deltaTime * playerRotSpeed);
        }
    }

    void CheckGrounded()
    {
        grounded = Physics.Raycast(playerTrans.position, Vector3.down, checkGroundDown, ~((1 << 2) + (1 << 8)));      
    }

    void CheckJump()
    {
        if (jumpInput && canJump && currentJumpCount <= maxJumpCount)
        {
            //canMove = false;
            currentJumpCount += jumpCountIntervals;
            butterflyJump.Play();
            myRigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            Debug.Log("Jumping");
        }

        if(currentJumpCount >= maxJumpCount)
        {
            canJump = false;
        }

        if (grounded == true)
        {
            currentJumpCount = minJumpCount;
            canJump = true;
            canMove = true;
        }

        jumpInput = false;
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastShooting();
        }
    }

    public void RaycastShooting()
    {
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Ray ray = playerCam.GetComponent<Camera>().ScreenPointToRay(new Vector2(x, y));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootingDistance, enemyLayer))
        {
            //enemyCont.takeDamage(weaponDamage);
            Debug.Log("Hit");
        }
    }
}

