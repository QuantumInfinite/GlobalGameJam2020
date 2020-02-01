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
    public float maxReturnTime = 2f;
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
        
    }
    public void OnTriggerEnter(Collider other)
    {
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!returning && Input.GetAxis("RewindTime") != 0)
            {
                //If we are not currently returning and they press the key
                returning = true;

                nearbyPieces = new List<ReturnToHome>();

                foreach (var piece in allPieces)
                {
                    if (Vector3.Distance(transform.position, piece.transform.position) < grabDistance)
                    {
                        nearbyPieces.Add(piece);
                        piece.StartCoroutine(piece.GoHome(returnSpeed, Time.realtimeSinceStartup + maxReturnTime));
                    }
                }
                Invoke("Check", maxReturnTime * 0.9f);
            }
            else if (returning && Input.GetAxis("RewindTime") == 0)
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
