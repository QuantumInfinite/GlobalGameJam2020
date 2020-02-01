using System.Collections;
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
    bool ableToRewind = false;

    enum State
    {
        Seperate, 
        Returning, 
        Assembled
    }
    State currentState;
    void Start()
    {
        completePiece.SetActive(false);
    }

    void Update()
    {
        if (!ableToRewind && (Input.GetAxis("RewindTime") == 0))
        {
            ableToRewind = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && ableToRewind)
        {
            if (currentState == State.Seperate && (Input.GetAxis("RewindTime") != 0))
            {
                //If we are not currently returning and they press the key
                currentState = State.Returning;

                nearbyPieces = new List<ReturnToHome>();

                foreach (var piece in allPieces)
                {
                    if (piece.gameObject.activeInHierarchy && Vector3.Distance(transform.position, piece.transform.position) < grabDistance)
                    {
                        Debug.Log("E");
                        nearbyPieces.Add(piece);
                        float endTime = Time.realtimeSinceStartup + maxReturnDuration;
                        float ascendTime = Time.realtimeSinceStartup + ascendDuration;
                        piece.StartCoroutine(piece.GoHome(returnSpeed, endTime, ascendSpeed, ascendTime));
                    }
                }
                Invoke("Check", maxReturnDuration * 0.9f);
            }
            else if (currentState == State.Returning && (Input.GetAxis("RewindTime") == 0))
            {
                Interupt();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (currentState == State.Returning)
            {
                Interupt();
            }
        }
    }

    void Check()
    {
        if(currentState == State.Seperate)
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
    void Interupt()
    {
        if (currentState == State.Returning)
        {
            CancelInvoke("Check");
            Fail();
        }
    }
    void Fail()
    {
        //Remove outstanding invokes
        CancelInvoke("Check");
        ableToRewind = false;
        Debug.Log("Failed");
        foreach (var piece in nearbyPieces)
        {
            piece.StopAllCoroutines();
            piece.Unlock();

            piece.rigid.AddExplosionForce(explosionForce, transform.position, 5f, -2f, ForceMode.Impulse);

            currentState = State.Seperate;
        }
        OnFail.Invoke();
    }

    void Success()
    {
        Debug.Log("Success!");
        completePiece.SetActive(true);
        currentState = State.Assembled;
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
