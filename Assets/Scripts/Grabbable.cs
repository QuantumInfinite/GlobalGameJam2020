﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public Sprite icon;
    Rigidbody rb;
    private Vector3 defaultScale; 

    private void Awake()
    {
        defaultScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        if (GravityGun.instance.currentGrabbed == null && !GravityGun.instance.isBusy)
        {
            SetCurrentObject();
        }
    }

    private void OnMouseEnter()
    {
        GravityGun.instance.cursor.sprite = GravityGun.instance.grabbableCursor;
    }
    private void OnMouseExit()
    {
        GravityGun.instance.cursor.sprite = GravityGun.instance.normalCursor;
    }

    public void SetCurrentObject()
    {
        GravityGun.instance.isBusy = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(MoveToPlayer());
    }

    public void UnsetCurrentObject()
    {
        GravityGun.instance.isBusy = true;
        transform.position = GravityGun.instance.dropSpot.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        StartCoroutine(SetScale(Size.GoBig));
    }

    public void StoreCurrentObject()
    {
        GravityGun.instance.isBusy = true;
        StartCoroutine(SetScale(Size.GoDisappear));
    }

    private IEnumerator MoveToPlayer()
    {
        float currentTime = 0;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = GravityGun.instance.smallScale;

        while (currentTime < GravityGun.instance.teleportSpeed)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, GravityGun.instance.gravitySpot.position, currentTime / GravityGun.instance.teleportSpeed);


            if(currentTime / GravityGun.instance.teleportSpeed > GravityGun.instance.startScalingWhenMoving)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, (currentTime - GravityGun.instance.startScalingWhenMoving) / (GravityGun.instance.teleportSpeed - GravityGun.instance.startScalingWhenMoving));
            }
            yield return new WaitForFixedUpdate();
        }
        GravityGun.instance.currentGrabbed = this;
        //StartCoroutine(SetScale(Size.GoSmall));

        transform.parent = GravityGun.instance.gravitySpot;
        GravityGun.instance.isBusy = false;
    }

    private IEnumerator SetScale(Size size)
    {
        float currentTime = 0;
        Vector3 startScale = transform.localScale;

        Vector3 endScale = Vector3.one;
        switch(size)
        {
            case Size.GoBig:
                endScale = defaultScale;
                break;
            case Size.GoDisappear:
                endScale = Vector3.zero;
                break;
            case Size.GoSmall:
                endScale = GravityGun.instance.smallScale;
                break;
                
        }
        while (currentTime < GravityGun.instance.scaleTime)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / GravityGun.instance.scaleTime);
            yield return new WaitForFixedUpdate();
        }

        if(size == Size.GoBig)
        {
            GravityGun.instance.currentGrabbed = null;
            rb.useGravity = true;
            transform.parent = null;
        }
        if(size == Size.GoDisappear)
        {
            gameObject.SetActive(false);
            if(GravityGun.instance.storedGrapped.Count >= GravityGun.instance.carryCapacity)
            {
                GravityGun.instance.storedGrapped[0].gameObject.SetActive(true);
                GravityGun.instance.storedGrapped[0].UnsetCurrentObject();
            }
        }

        GravityGun.instance.isBusy = false;
    }
}
