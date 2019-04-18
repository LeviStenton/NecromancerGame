using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    public GameObject player;

    PlayerSSUIController playerSSUI;
    GameObject camPiv;
    public GameObject myCam;
    public Transform camNearLeft;
    public Transform camNearRight;
    public Transform camFar;
    public GameObject myUI;
    public Transform uiNear;
    public Transform uiFar;
    public float mouseSensX;
    public float mouseSenseXMax;
    public float mouseSenseXStart;
    public float mouseSensY;
    public float mouseSenseYMax;
    public float mouseSenseYStart;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    Quaternion localXRotation;
    Quaternion localYRotation;
    public float viewRangeUp;
    public float viewRangeDown;
    private float rotateValueX;
    private float rotateValueY;
    public bool invertCam;
    public Vector3 offset;
    public float camHeight;
    public float camScrollSpeed;
    public float smoothSpeed;
    private float journeyLength;
    private float startTime;
    bool camSideSwitched;
    bool zoomedOut = false;
    public float centreVal;
    public float switchSpeed;
    public float zoomSpeed;
    public float switchTimer;
    int yInvert = 1;

    void Start()
    {
        playerSSUI = GameObject.FindGameObjectWithTag("SSUI").GetComponent<PlayerSSUIController>();

        camSideSwitched = false;
        myCam.transform.position = camNearRight.position;
        camPiv = this.gameObject;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        CameraRotation();
        FollowPlayer();
        CameraScroll();
        CamSideSwitch();
        camZoomSwitch();
    }

    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensX;
        rotX += (mouseY  * (playerSSUI.invertYToggle.isOn ? -yInvert : yInvert)) * mouseSensY;

        rotX = Mathf.Clamp(rotX, viewRangeDown, viewRangeUp);
        
        localYRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        camPiv.transform.localRotation = localYRotation;
    }

    public void FollowPlayer()
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void CameraScroll()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            zoomedOut = false;
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            zoomedOut = true;
            startTime = Time.time;
            journeyLength = Vector3.Distance(myCam.transform.position, camFar.transform.position);
        }      
    }

    public void camZoomSwitch()
    {
        if (!zoomedOut)
        {
            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, camSideSwitched ? camNearLeft.localPosition : camNearRight.localPosition, Time.deltaTime * zoomSpeed);
            myCam.transform.localRotation = Quaternion.Slerp(myCam.transform.localRotation, camSideSwitched ? camNearLeft.localRotation : camNearRight.localRotation, Time.deltaTime * (zoomSpeed * 2));
            myUI.transform.position = Vector3.Lerp(myUI.transform.position, uiNear.position, Time.deltaTime * zoomSpeed);
        }

        else if (zoomedOut)
        {
            float distCovered = (Time.time - startTime) * zoomSpeed;
            float fracJourney = distCovered / journeyLength;
            myCam.transform.position = Vector3.Lerp(myCam.transform.position, camFar.position, fracJourney);
            myCam.transform.localRotation = Quaternion.Slerp(myCam.transform.localRotation, camFar.localRotation, fracJourney);
            myUI.transform.position = Vector3.Lerp(myUI.transform.position, uiFar.position, fracJourney);
        }
    }

    public void CamSideSwitch()
    {
        if (Input.GetButtonDown("SwapSides") && !zoomedOut)
        {
            camSideSwitched = !camSideSwitched;
        }

        myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, camSideSwitched ? camNearLeft.localPosition : camNearRight.localPosition, Time.deltaTime * (switchSpeed * 2));
    }
}
