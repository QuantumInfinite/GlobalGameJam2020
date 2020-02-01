﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class FractureParent : MonoBehaviour
{
    [Header("Hookups")]
    public GameObject completePiece;
    public List<ReturnToHome> allPieces;

    [Header("Settings")]
    public float grabDistance = 5f;
    public float returnSpeed = 5;
    public float ascendSpeed = 1.5f;
    public float maxReturnDuration = 2f;
    public float ascendDuration = 1f;
    public float explosionForce = 2f;

    [Header("Events")]
    public UnityEvent OnFail;
    public UnityEvent OnSuccess;

    List<ReturnToHome> nearbyPieces;
    bool returning = false;
    void Start()
    {
        completePiece.SetActive(false);
    }

    void Update()
    {
        if (slowdown >= 0)
        {
            slowdown -= 1;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        
    }
    float slowdown = 1;
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && slowdown <= 0)
        {
            if (!returning && (Input.GetAxis("RewindTime") != 0))
            {
                //If we are not currently returning and they press the key
                returning = true;

                nearbyPieces = new List<ReturnToHome>();

                foreach (var piece in allPieces)
                {
                    if (Vector3.Distance(transform.position, piece.transform.position) < grabDistance)
                    {
                        nearbyPieces.Add(piece);
                        float endTime = Time.realtimeSinceStartup + maxReturnDuration;
                        float ascendTime = Time.realtimeSinceStartup + ascendDuration;
                        piece.StartCoroutine(piece.GoHome(returnSpeed, endTime, ascendSpeed, ascendTime));
                    }
                }
                Invoke("Check", maxReturnDuration * 0.9f);
            }
            else if (returning && (Input.GetAxis("RewindTime") == 0))
            {

                //If we are supposed to be returning but they let go of the key
                returning = false;
                CancelInvoke("Check");
                Fail();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (returning)
            {
                returning = false;
                CancelInvoke("Check");
                Fail();
            }
        }
    }

    void Check()
    {
        if(returning == false)
        {
            return;
        }

        if (nearbyPieces.Count == allPieces.Count && allPieces.All(x => x.gameObject.activeInHierarchy))
        {
            Success();
        }
        else
        {
            Fail();
        }

    }

    void Fail()
    {
        Debug.Log("Failed");
        foreach (var piece in nearbyPieces)
        {
            piece.StopAllCoroutines();
            piece.Unlock();

            piece.rigid.AddExplosionForce(explosionForce, transform.position, 5f, -2f, ForceMode.Impulse);

            returning = false;
        }
        OnFail.Invoke();
    }

    void Success()
    {
        Debug.Log("Success!");
        completePiece.SetActive(true);
        foreach (var piece in nearbyPieces)
        {
            piece.gameObject.SetActive(false);
        }
        OnSuccess.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
         Color color = Color.yellow;
        Gizmos.color = new Color(color.r, color.g, color.b, 0.2f);
        Gizmos.DrawSphere(transform.position, grabDistance);
    }
}
