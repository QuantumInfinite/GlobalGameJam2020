using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKinimatic : MonoBehaviour
{
    public List<Rigidbody> rigids;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var rig in rigids)
            {
                rig.isKinematic = false;
            }
        }
    }
}
