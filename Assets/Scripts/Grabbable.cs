using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        if (GravityGun.instance.currentGrabbed == null)
        {
            SetCurrentObject();
        }

    }

    public void SetCurrentObject()
    {
        rb.useGravity = false;
        StartCoroutine(MoveToPlayer());
    }

    public void UnsetCurrentObject()
    {
        GravityGun.instance.currentGrabbed = null;
        rb.useGravity = true;
        transform.parent = null;
    }

    private IEnumerator MoveToPlayer()
    {
        float currentTime = 0;
        Vector3 startPos = transform.position;
        while(currentTime < GravityGun.instance.teleportSpeed)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, GravityGun.instance.gravitySpot.position, currentTime / GravityGun.instance.teleportSpeed);
            yield return new WaitForFixedUpdate();
        }
        GravityGun.instance.currentGrabbed = this;
        transform.parent = GravityGun.instance.gravitySpot;
    }
}
