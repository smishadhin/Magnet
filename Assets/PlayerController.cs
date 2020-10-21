using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    private float keyBoardHorizontal, keyBoardVertical, joyStickHorizontal, joyStickVertical;
    public static PlayerController pc;
    public float speed = 3f;
    public float jumpSpeed = 5;
    private Rigidbody rb;

    public VariableJoystick js;
    public bool isRed = true;
    public GameObject body;
    private MeshRenderer playerMeshRenderer;
    public float distance = 1f, reflecDist;

    private float polechangePeriod = 5f;
    private float poleNextChage = 5f;

    private Vector2 touchBegan, touchMoving;
    private bool touchEnable;
    private float HorizontalInput, VerticalInput;
    
    
    int TapCount;
    public float MaxDubbleTapTime = 0.5f;
    float NewTime;
    
    private float boostTimer = 0f;
    private bool boosting = false;


    private void Awake()
    {
        InvokeRepeating("changePole", 0, 3.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //TapCount = 0;
        if (pc == null)
        {
            pc = this;
        }

        rb = this.GetComponent<Rigidbody>();
        playerMeshRenderer = body.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        keyBoardHorizontal = Input.GetAxisRaw("Horizontal");
        keyBoardVertical = Input.GetAxisRaw("Vertical");
        joyStickHorizontal = js.Horizontal;
        joyStickVertical = js.Vertical;


        keyBoardMovement(keyBoardHorizontal, keyBoardVertical);
        JoystickMovement(joyStickHorizontal, joyStickVertical);
        ColorToggle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }

        Touchdetection();
        TouchCtrl();
        doubleTap();
        speedBoost();
       
        
        Debug.Log("t count: "+Input.touchCount);
    }

    
    void speedBoost()
    {
        if (boosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer>=1)
            {
                speed = 5;
                boostTimer = 0;
                boosting = false;
            }
        }

        
       
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (MagneticField.magneticField.isRedm != isRed)
        {
            Debug.Log("A");
            Attraction(keyBoardHorizontal, keyBoardVertical);
        }
        else if (MagneticField.magneticField.isRedm == isRed)
        {
            Reflection(keyBoardHorizontal, keyBoardVertical);
        }
    }

    public void changePole()
    {
        isRed = !isRed;
    }

    void ColorToggle()
    {
        if (isRed)
        {
            playerMeshRenderer.material.color = Color.red;
        }
        else
        {
            playerMeshRenderer.material.color = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isRed = true;

            playerMeshRenderer.material.color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            isRed = false;

            playerMeshRenderer.material.color = Color.blue;
        }
    }


    void jump()
    {
        if (this.transform.position.y < 2.5f)
        {
            rb.AddForce(Vector3.up * jumpSpeed);
            Debug.Log("jump");
        }

        // rb.AddForce(Vector3.up*jumpSpeed,ForceMode.Impulse);
    }

    void keyBoardMovement(float horizontal, float vertical)
    {
        Vector3 playerMovement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }


    void JoystickMovement(float horizontal, float vertical)
    {
        if (horizontal > 0f)
        {
            horizontal = 1;
        }
        else if (horizontal < 0f)
        {
            horizontal = -1;
        }

        if (vertical > 0f)
        {
            vertical = 1;
        }
        else if (vertical < 0f)
        {
            vertical = -1;
        }


        Vector3 playerMovement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }

    public void Attraction(float horizontal, float vertical)
    {
        Transform enmypos = MagneticField.magneticField.ts;


        Vector3 pos = (this.transform.position - enmypos.position).normalized * distance + enmypos.position;
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, 0.25f);
    }

    public void Reflection(float horizontal, float vertical)
    {
        Transform enmypos = MagneticField.magneticField.ts;


        Vector3 pos = (this.transform.position - enmypos.position).normalized * reflecDist + enmypos.position;
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, 0.25f);
    }


    void Touchdetection()
    {
        // if (Input.touchCount > 1)
        // {
        //     Debug.Log("tc: "+ Input.touchCount);
        //     Touch t = Input.GetTouch(0);
        //     
        //     if (t.phase == TouchPhase.Began)
        //     {
        //         touchBegan = Input.mousePosition;
        //        // Debug.Log("v:  " + touchBegan);
        //     }
        //     
        //     if (t.phase == TouchPhase.Moved)
        //     {
        //         touchEnable = true;
        //     }
        //     else
        //     {
        //         touchEnable = false;
        //     }
        //     
        //     if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        //     {
        //         touchEnable = false;
        //     }
        // }
        // else
        // {
        //     touchEnable = false;
        // }

        if (joyStickHorizontal > 0 || joyStickVertical >0)
        {
            if (Input.touchCount > 1)
            {
               
                Debug.Log(" tc>1 "+ Input.touchCount);
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    touchBegan = Input.mousePosition;
                   // Debug.Log("v:  " + touchBegan);
                }

                if (t.phase == TouchPhase.Moved)
                {
                    touchEnable = true;
                }
                else
                {
                    touchEnable = false;
                }

                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    touchEnable = false;
                }
            }
        }
        else
        // {
        //     touchEnable = false;
        // }

        if (joyStickHorizontal < 1 || joyStickVertical < 1)
        {
            if (Input.touchCount > 0)
            {
               
                Debug.Log("tc>0"+ Input.touchCount);
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    touchBegan = Input.mousePosition;
                    //Debug.Log("v:  " + touchBegan);
                }

                if (t.phase == TouchPhase.Moved)
                {
                    touchEnable = true;
                }
                else
                {
                    touchEnable = false;
                }

                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    touchEnable = false;
                }
            }
        }
        else
        {
            touchEnable = false;
           
        }
    }


    void TouchCtrl()
    {
        if (touchEnable)
        {
            touchMoving = Input.mousePosition;

           

            if (touchMoving.y > touchBegan.y + 50f)
            {
                jump();
                touchEnable = false;
                //Input.touchCount = 0;
                // Debug.Log("swipe up");
                // Debug.Log("began:  " +touchBegan.y);
                // Debug.Log(touchMoving.y+"moving new:"+touchMoving.y+100f);
            }
        }
        else
        {
            HorizontalInput = 0;
            VerticalInput = 0;
            touchEnable = false;
           
        }
    }

    private float touchIn;
    private float touchOut;
    
    void doubleTap()
    {
        
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch (0);
             
            if (touch.phase == TouchPhase.Ended) {
                TapCount += 1;
            }
    
            if (TapCount == 1) {
                 
                NewTime = Time.time + MaxDubbleTapTime;
            }else if(TapCount == 2 && Time.time <= NewTime){
                 
                //Whatever you want after a dubble tap    
                boosting = true;
                speed = 15;
                print ("Dubble tap");
                Debug.Log("Dubble tap");
                     
                TapCount = 0;
            }
    
        }
        if (Time.time > NewTime) {
            TapCount = 0;
        }
        
        
    }
}