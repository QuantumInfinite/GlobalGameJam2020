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

    public bool isBusy = false;

    public float teleportSpeed = 1f;
    public float dropTimeBetween = 0.5f;

    public float scaleTime = 0.1f;

    [Range(0f, 1f)]
    public float startScalingWhenMoving = 0.9f;

    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f);

    private void Update()
    {
        //drop item
        if(!isBusy && currentGrabbed && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Dropped Item");
            currentGrabbed.UnsetCurrentObject();
        }
        //store item
        if (!isBusy && currentGrabbed && Input.GetMouseButtonDown(1))
        {
            storedGrapped.Add(currentGrabbed);
            currentGrabbed.StoreCurrentObject();
            currentGrabbed = null;
        }

        //drop all stored items
        if (!isBusy && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DropStoredItems());
        }
    }

    public IEnumerator DropStoredItems()
    {
        isBusy = true;
        foreach (var item in storedGrapped)
        {
            item.gameObject.SetActive(true);
            item.UnsetCurrentObject();
            yield return new WaitForSeconds(dropTimeBetween);
        }
        storedGrapped.Clear();
        isBusy = false;
    }
}

public enum Size {GoBig, GoSmall, GoDisappear}
