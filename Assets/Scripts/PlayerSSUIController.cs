using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSSUIController : MonoBehaviour {

    PlayerController playerController;
    CamController camController;
    public GameObject optionsMenu;

    public Toggle invertCam;
    public Slider camSensX;
    public Slider camSensY;

    bool optionsInput;
    bool toggleOptions;

    // Use this for initialization
    void Start ()
    {
        toggleOptions = false;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        camController = GameObject.FindGameObjectWithTag("CamPivot").GetComponent<CamController>();
        camSensX.value = camController.mouseSenseXStart;
        camSensY.value = camController.mouseSenseYStart;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("OptionsMenu"))
        {
            toggleOptions = !toggleOptions;            
        }

        optionsMenu.SetActive(toggleOptions ? true : false);
        Cursor.visible = toggleOptions ? true : false;
    }

    private void FixedUpdate()
    {
        OptionsMenu();
    }

    public void OptionsMenu()
    {
        camController.invertCam = invertCam;

        camController.mouseSensX = camSensX.value;        
        camSensX.maxValue = camController.mouseSenseXMax;
        camController.mouseSensY = camSensY.value;
        camSensY.maxValue = camController.mouseSenseYMax;
    }

}
