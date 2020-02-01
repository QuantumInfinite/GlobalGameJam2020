using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public static GravityGun instance;

    private void Awake()
    {
        instance = this;
    }

    public Grabbable currentGrabbed;

    public List<Grabbable> storedGrapped;

    public Transform gravitySpot;

    public float teleportSpeed = 1f;
    public float dropTimeBetween = 0.5f;

    private void Update()
    {
        //drop item
        if(currentGrabbed && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Dropped Item");
            currentGrabbed.UnsetCurrentObject();
        }
        //store item
        if (currentGrabbed && Input.GetMouseButtonDown(1))
        {
            storedGrapped.Add(currentGrabbed);
            currentGrabbed.gameObject.SetActive(false);
            currentGrabbed = null;
        }

        //drop all stored items
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DropStoredItems());
        }
    }

    public IEnumerator DropStoredItems()
    {
        foreach (var item in storedGrapped)
        {
            item.UnsetCurrentObject();
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(dropTimeBetween);
        }
        storedGrapped.Clear();
    }
}
