using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Missile : MonoBehaviour
{
    public GameManager gm;
    public GameObject obj;
    public float lifeTime;
    public ParticleSystem explosionPrefab;
    public AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {

        //explosion = GetComponentInChildren<ParticleSystem>();
        obj = GameObject.Find("GameManager");
        gm = obj.GetComponent<GameManager>();
        Debug.Log(obj + " " + gm) ;
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
        ParticleSystem explosion = Instantiate<ParticleSystem>(explosionPrefab);
        explosion.transform.position = collision.transform.position;
        explosionSound.Play();

        explosion.Play();
        Debug.Log("Just collided with " + collision.gameObject.name);
        if(collision.gameObject.name != "ForestTower_Blue(Clone)") gm.playMissileExplosion();

        gm.incrementDeadMissiles();
        Destroy(this.gameObject);
    }
    
}
