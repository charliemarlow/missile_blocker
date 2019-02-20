using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Missile : MonoBehaviour
{
    public GameManager gm;
    public GameObject obj;
    public float lifeTime; // missile lifetime
    public ParticleSystem explosionPrefab;
    public AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {

        // get a reference to game manager
        obj = GameObject.Find("GameManager");
        gm = obj.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // decrement missile lifetime
        lifeTime -= Time.deltaTime;
        if(this.transform.position.y < -10 || lifeTime < 0)
        {
            // destroy out of bound or old missiles
            gm.incrementDeadMissiles();
            Destroy(this.gameObject);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // create an explosion
        ParticleSystem explosion = Instantiate<ParticleSystem>(explosionPrefab);
        explosion.transform.position = this.transform.position;

        // play explosion noise
        explosionSound.Play();

        explosion.Play();
        // don't play missile explosion sound for the tower, it has its own noise
        if(collision.gameObject.name != "ForestTower_Blue(Clone)") gm.playMissileExplosion();

        // keep track of dead missiles
        gm.incrementDeadMissiles();
        Destroy(this.gameObject);
    }
    
}
