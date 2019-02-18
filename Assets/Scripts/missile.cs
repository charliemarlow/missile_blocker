using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Missile : MonoBehaviour
{
    public GameManager gm;
    public GameObject obj;
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("GameManager");
        gm = obj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(this.transform.position.y < -10 || lifeTime < 0)
        {
            gm.incrementDeadMissiles();
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        gm.incrementDeadMissiles();
        Destroy(this.gameObject);
    }
    
}
