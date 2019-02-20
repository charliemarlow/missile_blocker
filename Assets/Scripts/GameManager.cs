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
    public Transform spaceship;
    public AudioSource missileFireNoise;
    public AudioSource gameOverSound;
    public AudioSource missileExplosion;

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


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(missileSpawner());
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
    }

    // Update is called once per frame
    void Update()
    {
        // levels move on after a number of missiles have been fired
        activeMissileCount = totalMissileCount - deadMissiles;
        if(totalMissileCount >= missilesToNextLevel && isActiveLevel)
        {
            isActiveLevel = false;
            currentLevel++;
            timer = 6.0f;
        }

        // rotate the text constantly and rotate the spaceship
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.transform.Rotate(Vector3.up, 65.0f * Time.deltaTime);
        spaceship.transform.Rotate(Vector3.up, 20.0f * Time.deltaTime);

        if (!isActiveLevel)
        {
            // reset dead missiles
            deadMissiles = 0;
            timer -= Time.deltaTime;
            int time = (int)timer;

            if (gameEnd)
            {
                // game over specific message
                currentLevel = 1;
                text.text = "Game Over. Level " + currentLevel + " starting in " + time + " seconds";
            }
            else
            {
                // level upgrade message
                text.text = "Level " + currentLevel + " starting in " + time + " seconds";
            }

            
            if(timer <= 0.0f)
            {
                if (buildingCount == 0)
                {
                    // create a new building if it doesn't exist
                    Building newBuilding = Instantiate<Building>(buildingPrefab);
                    buildingCount++;
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
            // no text during the level
            text.text = " ";
        }
    
    }

    public void playMissileExplosion(){
        missileExplosion.Play();
    }

    public void incrementDeadMissiles()
    {
        deadMissiles++;
    }

    public void gameOver(Building b)
    {
        // play game over noise
        gameOverSound.Play();
        if (buildingCount > 0)
        {
            buildingCount--;
        }
        isActiveLevel = false;
        timer = 6.0f;
        gameEnd = true;

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
        rb.velocity = (target.position - m.transform.position).normalized * missileSpeed;

        Vector3 targetDir = target.position - m.transform.position;
        float step = missileSpeed * Time.deltaTime;
        Quaternion q = Quaternion.LookRotation(targetDir);
        rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, q, .1f);

    }

    Missile spawnMissile()
    {
        //Create a missile
        Missile m = Instantiate<Missile>(missilePrefab);
        if(m == null) Debug.Log("null missile");
        Vector3 randomRight = Random.Range(-1.0f, 1.0f) * missileSpawnRadius * missileSpawnOrigin.right;
        Vector3 randomForward = Random.Range(-1.0f, 1.0f) * missileSpawnRadius * missileSpawnOrigin.forward;
        // assign its position a random offset from the spaceship
        m.transform.position = missileSpawnOrigin.position + randomRight + randomForward;
        return m;
    }
}
