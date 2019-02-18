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

        if(indexTriggerState > .5 && Time.time - lastFire > 1 )
        {
            fire();
            lastFire = Time.time;
            
        }
    }

    /*
     * // Frame update example: Draws a 10 meter long green line from the position for 1 frame.
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
     */

    void fire()
    {
        laserLine.enabled = true;
        Debug.Log("Pew!");
        gunshot.Play();
        RaycastHit hit;
        Vector3 forward = gunBarrel.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(gunBarrel.position, forward, Color.green);
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit)){
            Debug.Log("You hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Missile"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("no hit");
        }
        laserLine.enabled = false;
    } 

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

    bool isLeftController()
    {
        if (grabbableGun.grabbedBy == left)
        {
            return true;
        }
        return false;
    }
}
