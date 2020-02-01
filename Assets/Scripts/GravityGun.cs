using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityGun : MonoBehaviour
{
    public static GravityGun instance;
    public Sprite normalCursor;
    public Sprite grabbableCursor;
    public Image cursor;

    public Image[] pickups;

    private void Awake()
    {
        instance = this;
        carryCapacity = pickups.Length;
    }

    public Grabbable currentGrabbed;

    public List<Grabbable> storedGrapped;

    public Transform gravitySpot;
    public Transform dropSpot;

    public bool isBusy = false;

    public float teleportSpeed = 1f;

    public float scaleTime = 0.1f;

    [Range(0f, 1f)]
    public float startScalingWhenMoving = 0.9f;

    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f);

    public int carryCapacity;

    public ParticleSystem ps_spawnObject;
    public ParticleSystem ps_storeObject;

    private void Update()
    {
        //drop item
        if(!isBusy && currentGrabbed && Input.GetMouseButtonDown(0))
        {
            currentGrabbed.UnsetCurrentObject();
        }
        //store item
        if (!isBusy && currentGrabbed && Input.GetMouseButtonDown(1))
        {
            storedGrapped.Add(currentGrabbed);
            ps_storeObject.Play();
            currentGrabbed.StoreCurrentObject();
            currentGrabbed = null;
        }

        //drops a single object
        if (!isBusy && Input.GetKeyDown(KeyCode.E) && storedGrapped.Count > 0)
        {
            storedGrapped[0].gameObject.SetActive(true);
            storedGrapped[0].UnsetCurrentObject();
            storedGrapped.Remove(storedGrapped[0]);
        }

        for(int i = 0; i < pickups.Length; i++)
        {
            if(storedGrapped.Count > i)
            {
                pickups[i].sprite = storedGrapped[i].icon;
            }
            pickups[i].gameObject.SetActive(storedGrapped.Count > i);
            
        }
    }

}

public enum Size {GoBig, GoSmall, GoDisappear}
