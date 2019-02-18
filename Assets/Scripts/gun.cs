using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public OVRGrabbable grabbableGun;
    private float lastFire;
    public OVRGrabber left;
    public OVRGrabber right;
    // Start is called before the first frame update
    void Start() => lastFire = Time.time;

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

    void fire()
    {
        Debug.Log("Pew!");
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
