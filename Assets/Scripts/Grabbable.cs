﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public Sprite icon;
    public Rigidbody rb;
    private Vector3 defaultScale;
    public Vector3 startLocation;

    private void Awake()
    {
        defaultScale = transform.lossyScale;
        startLocation = transform.position;

        rb = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        if (GravityGun.instance.currentGrabbed == null && !GravityGun.instance.isBusy)
        {
            GravityGun.instance.audioSource.clip = GravityGun.instance.startMoveSFX;
            GravityGun.instance.audioSource.Play();
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
        GravityGun.instance.showBeam = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(MoveToPlayer());
    }

    public void UnsetCurrentObject(bool fromDrop, float scaleTime)
    {
        GravityGun.instance.isBusy = true;
        transform.position = fromDrop ? GravityGun.instance.dropSpot.position : GravityGun.instance.gravitySpot.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        if (fromDrop)
        {
            GravityGun.instance.audioSource.clip = GravityGun.instance.spawnInSFX;
            GravityGun.instance.audioSource.Play();
            GravityGun.instance.ps_spawnObject.Play();
        }
        StartCoroutine(SetScale(Size.GoBig, scaleTime));
    }

    public void StoreCurrentObject()
    {
        GravityGun.instance.isBusy = true;
        StartCoroutine(SetScale(Size.GoDisappear, GravityGun.instance.scaleTime));
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
        GravityGun.instance.showBeam = false;
        transform.parent = GravityGun.instance.gravitySpot;
        transform.position = GravityGun.instance.gravitySpot.position;
        GravityGun.instance.SetMesh(this);
        GravityGun.instance.isBusy = false;
    }

    private IEnumerator SetScale(Size size, float scaleTime)
    {
        float currentTime = 0;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;
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
        while (currentTime < scaleTime)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / scaleTime);
            if(startPos != GravityGun.instance.dropSpot.position)
            {
                transform.position = Vector3.Lerp(startPos, GravityGun.instance.dropSpot.position, currentTime / scaleTime);
            }
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
                GravityGun.instance.storedGrapped[0].UnsetCurrentObject(true, scaleTime);
            }
        }

        GravityGun.instance.isBusy = false;
    }
}
