using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public FractureParent connectedFracture;

    Collider myCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        connectedFracture.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        connectedFracture.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        connectedFracture.OnTriggerExit(other);
    }
}
