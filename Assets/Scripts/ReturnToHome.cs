﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider), typeof(Rigidbody), typeof(Grabbable))]
public class ReturnToHome : MonoBehaviour
{
    public Transform goalOverride;
    Vector3 goalPos;
    Quaternion goalRot;
    [HideInInspector]
    public Rigidbody rigid;
    Collider myCollider;
    Grabbable grabbable;
    AudioSource audioSource;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        grabbable = GetComponent<Grabbable>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (goalOverride == null)
        {
            goalPos = transform.position;
            goalRot = transform.rotation;
        }
        else
        {
            goalPos = goalOverride.position;
            goalRot = goalOverride.rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2 && audioSource != null)
        {
            audioSource.volume = collision.relativeVelocity.magnitude / 5;
            audioSource.Play();
        }
    }

    public IEnumerator GoHome(float returnSpeed, float endTime, float ascendSpeed, float ascendTime)
    {
        rigid.useGravity = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        grabbable.enabled = false;


        bool stillCorrecting = true;
        while (stillCorrecting && Time.realtimeSinceStartup < ascendTime)
        {
            float translation = (goalPos.y - transform.position.y) * ascendSpeed * Time.deltaTime;
            rigid.MovePosition(transform.position + new Vector3(0, translation, 0));

            transform.rotation = goalRot;
            yield return new WaitForEndOfFrame();
        }


        while (stillCorrecting && Time.realtimeSinceStartup < endTime)
        {     
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            stillCorrecting = false;
            if (Vector3.Distance(transform.position, goalPos) > 0.01)
            {
                //Movement
                Vector3 translation = (goalPos - transform.position) * returnSpeed * Time.deltaTime;

                rigid.MovePosition(transform.position + translation);

                stillCorrecting = true;
            }

            if(Quaternion.Angle(transform.rotation, goalRot) > 20)
            {
                //Rotation
                transform.rotation = goalRot;
                stillCorrecting = true;
            }

            yield return new WaitForEndOfFrame();
        }

        LockDown();
        
        yield return null;
    }
    

    public void LockDown()
    {
        myCollider.enabled = false;

        transform.position = goalPos;
        transform.rotation = goalRot;

        rigid.isKinematic = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    public void Unlock()
    {
        rigid.useGravity = true;
        grabbable.enabled = true;
        myCollider.enabled = true;

        rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints.None;
    }
}
