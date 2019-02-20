using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 10; // bullet dies after travelling too long
        
    }

    // Update is called once per frame
    void Update()
    {
        // destroy old bullets
        lifetime -= Time.deltaTime;

        if(lifetime < 0) Destroy(this.gameObject);
        
    }

    void OnCollisionEnter(Collision collision){
        
        // don't destroy the bullet when it hits the gun
        if(collision.gameObject.name != "Gun") Destroy(this.gameObject);     

        // destroy the missile if the bullet hits it  
        if(collision.gameObject.name == "missile3(clone)") Destroy(collision.gameObject);
    }
    
}
