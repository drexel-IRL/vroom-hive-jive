﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;
    private Collider collider;
    private GameObject leader;
    private Transform parentTransform;

    private bool isFollowing = false;

    public float followSpeed = 4f;
    public float stoppingDistance = 2.0f;

    public float rotationalSlerpDamping = 1.0f;

    public ParticleSystem deathParticles;

    #endregion



    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        parentTransform = transform.parent;
    }

    /*
     * If the player collides with this object, it will begin following them in some manner, determined 
     * by the FixedUpdate method.
     */
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Bike" && isFollowing == false)
        {
            Debug.Log("Bee Collided!!");
            isFollowing = true;
            // collider.enabled = false;
            leader = FindObjectOfType<Leader>().GetFormationPosition(this);
        }
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            FollowPlayerPositionLerp();
        }
    }

    /* ==================================================================================================
     * The following "Follow" methods are various options to be used in the FixedUpdate of this object to 
     * determine how it will follow a leader object. Once it is decided which movement type is best, all 
     * others may be removed.
     */


    /* 
     * This object will turn towards its leader and move towards the leader's current location, as long 
     * as it is not closer than the stoppingDistance
     */
    void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, leader.transform.position) < stoppingDistance)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            transform.LookAt(leader.transform);
            rb.velocity = transform.forward * followSpeed;
        }
    }


    /* 
     * This object's position will be assigned that of its leader
     */
    void FollowPlayerPosition()
    {
        transform.position = leader.transform.position;
    }


    /*
     * Similar to FollowPlayerPostion, but uses a lerp function to smooth the movement between the  
     * different positions.
     */
    void FollowPlayerPositionLerp()
    {
        Vector3 lookPos = leader.transform.position - parentTransform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, rotation, Time.deltaTime * rotationalSlerpDamping);

        parentTransform.position = Vector3.Lerp(parentTransform.position, leader.transform.position, Time.deltaTime * followSpeed);
    }
    
    public void StopFollowing()
    {
        isFollowing = false;
        parentTransform.gameObject.SetActive(false);
    }

    #endregion

}