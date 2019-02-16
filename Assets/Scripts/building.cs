using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        Missile attacker = rb.GetComponent<Missile>();
        if (rb != null && attacker != null)
        {
            Debug.Log("Hit by " + collision.gameObject);
            //Destroy(this.gameObject);

        }
        // handle missile is null
        // handle collision rigidbody is null



    }
}
