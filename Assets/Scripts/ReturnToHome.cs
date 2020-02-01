using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToHome : MonoBehaviour
{
    Vector3 origionalPos;
    Quaternion origionalRot;
    Vector3 origionalRotation;

    public Rigidbody rigid;
    Collider myCollider;
    Grabbable grabbable;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        grabbable = GetComponent<Grabbable>();
    }

    void Start()
    {
        origionalPos = transform.position;
        origionalRot = transform.rotation;
        origionalRotation = transform.rotation.eulerAngles;        
    }

    public IEnumerator GoHome(float returnSpeed, float forceEndTime)
    {
        rigid.useGravity = false;
        grabbable.enabled = false;
        //float forceEndTime = Time.realtimeSinceStartup + 2f;
        bool stillCorrecting = true;
        while (stillCorrecting && Time.realtimeSinceStartup < forceEndTime)
        {     
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            

           // Debug.Log(Vector3.Distance(transform.position, origionalPos) + " | " + Quaternion.Angle(transform.rotation, origionalRot));


            stillCorrecting = false;
            if (Vector3.Distance(transform.position, origionalPos) > 0.01)
            {
                //Movement
                Vector3 translation = (origionalPos - transform.position) * returnSpeed * Time.deltaTime;

                rigid.MovePosition(transform.position + translation);

                stillCorrecting = true;
            }
            if(Quaternion.Angle(transform.rotation, origionalRot) > 20)
            {
                //Rotation

                //Vector3 tran = (origionalRot.eulerAngles - transform.rotation.eulerAngles) * returnSpeed * Time.deltaTime;

                transform.rotation = origionalRot;
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

        transform.position = origionalPos;
        transform.rotation = origionalRot;

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
