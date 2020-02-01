using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToHome : MonoBehaviour
{
    public Vector3 origionalPos;
    Quaternion origionalRot;
    Vector3 origionalRotation;
    bool returning = false;
    float returnSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        origionalPos = transform.position;
        origionalRot = transform.rotation;
        origionalRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GoHome()
    {
        returning = true;
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        while (returning)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            Debug.Log(Vector3.Distance(transform.position, origionalPos) + " | " + Quaternion.Angle(transform.rotation, origionalRot));
            int breaker = 0;
            if (Vector3.Distance(transform.position, origionalPos) > 0.01)
            {
                //Movement
                Vector3 translation = (origionalPos - transform.position) * returnSpeed * Time.deltaTime;
            
                rigid.MovePosition(transform.position + translation);
                breaker++;
            }
            if(Quaternion.Angle(transform.rotation, origionalRot) > 20)
            {
                //Rotation

                Vector3 tran = (origionalRot.eulerAngles - transform.rotation.eulerAngles) * returnSpeed * Time.deltaTime;

                transform.rotation = origionalRot;



                breaker++;
            }

            if (breaker == 0)
            {
                returning = false;
            }
            //Vector3 change = (Vector3.RotateTowards(transform.rotation.eulerAngles, origionalRotation, returnSpeed * 100 * Time.deltaTime, 0.0f));
            //transform.rotation = Quaternion.Lerp(transform.rotation, origionalRot, Time.time * returnSpeed / 100);
            //transform.rotation = Quaternion.LookRotation(change);
            //Debug.Log(Quaternion.RotateTowards(transform.rotation, origionalRot, returnSpeed / 100));

            //rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, origionalRot, returnSpeed * Time.deltaTime));

            yield return new WaitForEndOfFrame();
        }
        //Debug.Break();
        GetComponent<Collider>().enabled = false;
        transform.position = (origionalPos);
        transform.rotation = origionalRot;

        rigid.isKinematic = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        yield return null;
    }
}
