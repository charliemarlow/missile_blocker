using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    GrabObject currentObj = null;
    public OVRInput.Controller myController;
    public float triggerThreshold;
    public float releaseTriggerThreshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        GrabObject obj = rb.GetComponent<GrabObject>();
        if (obj == null) return;
        Debug.Log("Rb = " + rb.name + "\nObj =" + obj.name);
        float triggerValue;
        // change to primary hand trigger if gun
        if (myController == OVRInput.Controller.LTouch)
        {
            triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        }
        else
        {
            triggerValue = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        }

        Debug.Log(triggerValue);
        if(currentObj == null && triggerValue > triggerThreshold )
        {
            Debug.Log("In if");
            currentObj = obj;
            currentObj.pickedUp(this.transform);
        }

        if(currentObj != null && triggerValue < releaseTriggerThreshold)
        {
            currentObj.released(this.transform);
            currentObj = null;
            /*
            Vector3 vel = OVRInput.GetLocalControllerVelocity(myController);
            rb.velocity = vel;
            */
        }
    }
}
