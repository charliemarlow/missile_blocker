using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool missileSpawnerActive;
    public float missileSpawnPeriod_sec;
    public float missileSpawnRadius;
    public float missileSpeed;
    public Transform missileSpawnOrigin;
    public Transform target;
    public Missile missilePrefab;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(missileSpawner());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator missileSpawner()
    {
        while (missileSpawnerActive)
        {
            // spawn Missiles
            Missile m = spawnMissile();
            if (target != null)
            {
                targetMissile(m);
            }
            yield return new WaitForSeconds(missileSpawnPeriod_sec);
        }
        yield return null;
    }
    void targetMissile(Missile m)
    {
        Rigidbody rb = m.GetComponent<Rigidbody>();
        rb.velocity = (target.position - m.transform.position).normalized * missileSpeed;
    }

    Missile spawnMissile()
    {
        //Create a missile
        Missile m = Instantiate<Missile>(missilePrefab);
        Vector3 randomRight = Random.Range(-1.0f, 1.0f) * missileSpawnRadius * missileSpawnOrigin.right;
        Vector3 randomForward = Random.Range(-1.0f, 1.0f) * missileSpawnRadius * missileSpawnOrigin.forward;
        // assign its position a random offset from the spaceship
        m.transform.position = missileSpawnOrigin.position + randomRight + randomForward;
        return m;
    }
}
