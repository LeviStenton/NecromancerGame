using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    public GameObject player;

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
    public float viewRange;
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

    void Start()
    {
        camSideSwitched = false;
        myCam.transform.position = camNearRight.position;
        camPiv = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        CameraRotation();
        FollowPlayer();
        CameraScroll();
        CamSideSwitch();
        camZoomSwitch();
        //CameraRange();
    }

    public void CameraRotation()
    {
        Debug.Log(invertCam);
        if (invertCam)
            mouseSensY = -mouseSensY;

        /*float mouseSpeedX = Input.GetAxis("Mouse Y") * mouseSensY;
        float mouseSpeedY = Input.GetAxis("Mouse X") * mouseSensX;
        rotateValueX = new Vector3(mouseSpeedX * -1, 0, 0);
        transform.eulerAngles = transform.eulerAngles + rotateValueX;
        rotateValueY = new Vector3(0, mouseSpeedY * +1, 0);
        transform.eulerAngles = transform.eulerAngles + rotateValueY;*/

        float mouseSpeedX = Input.GetAxis("Mouse X");
        float mouseSpeedY = -Input.GetAxis("Mouse Y");

        rotateValueX += mouseSpeedX * mouseSensX;
        rotateValueY += mouseSpeedY * mouseSensY;

        rotateValueX = Mathf.Clamp(rotateValueX, -viewRange, viewRange);

        Quaternion localRotation = Quaternion.Euler(rotateValueX, 0.0f, 0.0f);
        transform.localRotation = localRotation;
    }

    /*public void CameraRange()
    {
        camPiv.transform.localEulerAngles = new Vector3(Mathf.Clamp(camPiv.transform.localEulerAngles.x, -viewRange, viewRange), camPiv.transform.localEulerAngles.y, camPiv.transform.localEulerAngles.z);
    }*/

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
            myCam.transform.localPosition = Vector3.Lerp(myCam.transform.localPosition, camFar.localPosition, Time.deltaTime * zoomSpeed);
            myCam.transform.localRotation = Quaternion.Slerp(myCam.transform.localRotation, camFar.localRotation, Time.deltaTime * zoomSpeed);
            myUI.transform.position = Vector3.Lerp(myUI.transform.position, uiFar.position, Time.deltaTime * zoomSpeed);
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
