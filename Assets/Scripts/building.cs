﻿using System.Collections;
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
        obj = GameObject.Find("GameManager");
        manager = obj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
