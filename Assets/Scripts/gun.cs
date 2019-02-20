using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public OVRGrabbable grabbableGun;
    private float lastFire;
    public OVRGrabber left;
    public OVRGrabber right;
    public Transform gunBarrel;
    public AudioSource gunshot;
    public LineRenderer laserLine;

    public float bulletSpeed;
    public Bullet bulletPrefab;
    public Transform bulletSpawnOrigin;

    // Start is called before the first frame update
    void Start()
    {
        lastFire = Time.time;
        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!grabbableGun.isGrabbed) return;

        bool isLeft = isLeftController();
        float indexTriggerState = getIndexTriggerState(isLeft);

        // only fire once per half second
        if(indexTriggerState > .5 && Time.time - lastFire > .5 )
        {
            fire();
            lastFire = Time.time;
            
        }
    }

    void fire()
    {
        // play gunshot noise
        gunshot.Play();

        // create a bullet
        Bullet b = Instantiate(bulletPrefab, bulletSpawnOrigin.position, bulletSpawnOrigin.rotation);
        Debug.Log(b);
        Rigidbody rb = b.GetComponent<Rigidbody>();

        // shoot bullet
        rb.AddForce(gunBarrel.transform.forward * 3000);
        rb.transform.position = bulletSpawnOrigin.position;
        rb.transform.rotation = bulletSpawnOrigin.rotation;
    } 

    // gets the index trigger state based on the type of controller
    float getIndexTriggerState(bool isLeft)
    {
        float indexTriggerState = 0.0f;
        if (isLeft)
        {
            indexTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        }
        else
        {
            indexTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        }
        return indexTriggerState;
    }

    // returns true if the grabber is the left controller
    bool isLeftController()
    {
        if (grabbableGun.grabbedBy == left)
        {
            return true;
        }
        return false;
    }
}
