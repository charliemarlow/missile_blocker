using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool missileSpawnerActive;
    public float missileSpawnPeriod_sec;
    public float missileSpawnRadius;
    public float missileSpeedBase;
    public int missilesPerRound;
    public Transform missileSpawnOrigin;
    public Transform target;
    public Missile missilePrefab;
    public Building buildingPrefab;
    public TextMesh text;
    public Transform boundsCollider;
    public AudioSource missileFireNoise;
    public AudioSource gameOverSound;

    private int activeMissileCount;
    private int currentLevel;
    private bool isActiveLevel;
    private int totalMissileCount;
    private float missileSpeed;
    private int missilesToNextLevel;
    private int deadMissiles;
    private float timer;
    private bool gameEnd;
    private int buildingCount;
    //private Bounds gameBound;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(missileSpawner());
        //StartCoroutine(rotateText());
        activeMissileCount = 0;
        totalMissileCount = 0; 
        isActiveLevel = false;
        currentLevel = 1;
        timer = 7.0f;
        deadMissiles = 0;
        missileSpawnerActive = true;
        missilesToNextLevel = missilesPerRound * currentLevel;
        gameEnd = false;
        buildingCount = 0;
        //gameBound = new Bounds();
        //gameBound.Encapsulate(boundsCollider.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        activeMissileCount = totalMissileCount - deadMissiles;
        if(totalMissileCount >= missilesToNextLevel && isActiveLevel)
        {
            isActiveLevel = false;
            currentLevel++;
            timer = 6.0f;
        }

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.transform.Rotate(Vector3.up, 65.0f * Time.deltaTime);

        if (!isActiveLevel)
        {
            deadMissiles = 0;
            timer -= Time.deltaTime;
            int time = (int)timer;

            if (gameEnd)
            {
                
                currentLevel = 1;
                text.text = "Game Over. Level " + currentLevel + " starting in " + time + " seconds";
            }
            else
            {
                text.text = "Level " + currentLevel + " starting in " + time + " seconds";
            }

            
            if(timer <= 0.0f)
            {
                if (buildingCount == 0)
                {
                    Debug.Log("Instantiating");
                    Building newBuilding = Instantiate<Building>(buildingPrefab);
                    buildingCount++;
                    Debug.Log("B count after instantiation "+ buildingCount);
                    target = newBuilding.transform;
                }
                isActiveLevel = true;
                totalMissileCount = 0;
                missilesToNextLevel = missilesPerRound * currentLevel;
                missileSpeed = missileSpeedBase * currentLevel;
                gameEnd = false;
            }
        }
        else
        {
            text.text = " ";
        }
    
    }

    public void incrementDeadMissiles()
    {
        deadMissiles++;
    }

    public void gameOver(Building b)
    {
        gameOverSound.Play();
        Debug.Log("Destroying");
        if (buildingCount > 0)
        {
            buildingCount--;
        }
        isActiveLevel = false;
        timer = 6.0f;
        gameEnd = true;
        Debug.Log("B count in gameover: " + buildingCount);
       /* if (buildingCount == 0)
        {
            Debug.Log("Instantiating");
            Building newBuilding = Instantiate<Building>(buildingPrefab);
            buildingCount++;
            target = b.transform;
        }
        */

    }

    IEnumerator rotateText()
    {
        while (true)
        {
            RectTransform textRect = text.GetComponent<RectTransform>();
            transform.Rotate(Vector3.up, 20.0f * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator missileSpawner()
    {
        while (missileSpawnerActive )
        {

            // spawn Missiles
            if (isActiveLevel)
            {
                Missile m = spawnMissile();
                missileFireNoise.Play();
                activeMissileCount++;
                totalMissileCount++;
                if (target != null)
                {
                    targetMissile(m);
                }
            }
            
            yield return new WaitForSeconds(missileSpawnPeriod_sec);
        }
        yield return null;
    }

    void targetMissile(Missile m)
    {
        
        Rigidbody rb = m.GetComponent<Rigidbody>();
        /*
        m.transform.LookAt(target.position);
        m.transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);

        */
        rb.velocity = (target.position - m.transform.position).normalized * missileSpeed;


        Vector3 targetDir = target.position - m.transform.position;

        // The step size is equal to speed times frame time.
        float step = missileSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(m.transform.right, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        //m.transform.rotation = Quaternion.LookRotation(newDir);
        /*
        Vector3 targetDir = target.position - rb.transform.position;
        float step = missileSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(rb.transform.forward, targetDir, step, 0.0f);
        rb.transform.rotation = Quaternion.LookRotation(newDir);
        rb.transform.position = Vector3.MoveTowards(rb.transform.position, rb.position, step);
        
        Rigidbody rb = m.GetComponent<Rigidbody>();
        Vector3 direction = (target.position - rb.transform.position).normalized;
        Vector3 vel = direction * missileSpeed;

        Vector3 force = vel - rb.velocity;
        rb.velocity = force;
        */

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
