using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    PlayerController playerController;
    public Camera myCam;
    public Text followerCounter;
    public Slider healthBar;    

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(transform.position + myCam.transform.rotation * Vector3.forward, myCam.transform.rotation * Vector3.up);
	}

    private void FixedUpdate()
    {
        FollowerCounting();
        HealthBar();
    }

    public void HealthBar()
    {
        healthBar.maxValue = playerController.maxHealth;
        healthBar.value = playerController.currentHealth;
    }

    void FollowerCounting()
    {
        followerCounter.text = ("") + playerController.followerCount;
    }
}
