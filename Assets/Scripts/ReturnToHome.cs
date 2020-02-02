using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider), typeof(Rigidbody), typeof(Grabbable))]
[RequireComponent(typeof(LineRenderer))]
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
    LineRenderer lineRenderer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        grabbable = GetComponent<Grabbable>();
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
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

        //LineRenderer stuff
        lineRenderer.SetPosition(0, goalPos);
        lineRenderer.enabled = false;
    }
    private void Update()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }

        if (transform.position.y < -10)
        {
            Debug.Log(gameObject.name + " fell out of map");
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
        grabbable.enabled = false;

        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        rigid.useGravity = false;
        myCollider.enabled = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;
        float currentTime = 0;
        Quaternion startRot = transform.rotation;

        while (Time.realtimeSinceStartup < ascendTime)
        {
            float translation = (goalPos.y - transform.position.y) * ascendSpeed * Time.deltaTime;
            rigid.MovePosition(transform.position + new Vector3(0, translation, 0));

            currentTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, goalRot, currentTime / (ascendTime - Time.realtimeSinceStartup));
            
            //lineRenderer.SetPosition(1, transform.position);
            yield return new WaitForEndOfFrame();
        }

        bool stillCorrecting = true;

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

        lineRenderer.enabled = false;
    }
    
    public void Unlock()
    {
        rigid.useGravity = true;
        grabbable.enabled = true;
        myCollider.enabled = true;

        rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints.None;

        lineRenderer.enabled = false;
    }
}
