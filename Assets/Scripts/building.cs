using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject obj;
    public GameManager manager;
    public AudioSource explosion;
    // Start is called before the first frame update
    void Start()
    {
        // get reference to game manager
        obj = GameObject.Find("GameManager");
        manager = obj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // these are the bounds
        // if the tower leaves the table, the game ends
        if(this.transform.position.z <= -.8 || this.transform.position.z >= .8 ||
        this.transform.position.x >= 1.1 || this.transform.position.x <= -1.1 ||
        this.transform.position.y <= .5){
            Destroy(this.gameObject);
            manager.gameOver(this);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;
        Missile attacker = rb.GetComponent<Missile>();
        if (attacker == null) return;

        if (rb != null && attacker != null)
        {
            Destroy(this.gameObject);
            manager.gameOver(this);
        }

    }
}
