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

    public AudioClip slurpSFX;
    public AudioClip startMoveSFX;
    public AudioClip spawnInSFX;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        carryCapacity = pickups.Length;
    }

    public Grabbable currentGrabbed;

    public List<Grabbable> storedGrapped = new List<Grabbable>();

    public Transform gravitySpot;
    public Transform dropSpot;

    public bool isBusy = false;

    public float teleportSpeed = 1f;

    public float scaleTime = 0.1f;

    [Range(0f, 1f)]
    public float startScalingWhenMoving = 0.9f;

    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f);

    public int carryCapacity;

    public bool showBeam = false;

    public GameObject beam;
    public ParticleSystem ps_spawnObject;
    public ParticleSystem ps_storeObject;

    public float throwSpeed = 1000f;

    private void Update()
    {
        //drop item
        if(!isBusy && currentGrabbed && Input.GetMouseButtonDown(0))
        {
            showBeam = false;
            currentGrabbed.UnsetCurrentObject(false, scaleTime);
        }
        //store item
        if (!isBusy && currentGrabbed && Input.GetMouseButtonDown(1))
        {
            showBeam = false;
            storedGrapped.Add(currentGrabbed);
            ps_storeObject.Play();
            audioSource.clip = slurpSFX;
            audioSource.Play();
            currentGrabbed.StoreCurrentObject();
            currentGrabbed = null;
        }

        //drops a single object
        if (!isBusy && !currentGrabbed && Input.GetMouseButtonDown(1) && storedGrapped.Count > 0)
        {
            storedGrapped[0].gameObject.SetActive(true);
            storedGrapped[0].UnsetCurrentObject(true, scaleTime);
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

        //throwing object
        if(!isBusy && currentGrabbed && Input.GetMouseButtonDown(2))
        {
            showBeam = false;
            currentGrabbed.UnsetCurrentObject(false, 0.1f);
            currentGrabbed.rb.AddForce(transform.forward * throwSpeed);
        }

        beam.SetActive(showBeam);
    }

}

public enum Size {GoBig, GoSmall, GoDisappear}
