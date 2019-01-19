using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour {

    public ParticleSystem energyExplosion;
    public ParticleSystem butterflyExplosion;	

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "TestArea")
        {
            energyExplosion.Play();
            butterflyExplosion.Play();
        }        
    }
}
