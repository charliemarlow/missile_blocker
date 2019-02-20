using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        Debug.Log("collid`!!");
        if(collision.gameObject.name != "Gun"){
            Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);       
        }
    }
    
}
