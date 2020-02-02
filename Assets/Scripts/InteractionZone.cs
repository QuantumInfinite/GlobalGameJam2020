using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionZone : MonoBehaviour
{
    public FractureParent connectedFracture;
    public Image cursor;
    public Sprite newCursor;
        public Sprite oldCursor;

    Collider myCollider;

    

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        connectedFracture.OnTriggerEnter(other);
        cursor.sprite = newCursor;
    }
    private void OnTriggerStay(Collider other)
    {
        connectedFracture.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        connectedFracture.OnTriggerExit(other);
        cursor.sprite = oldCursor;
    }
}
