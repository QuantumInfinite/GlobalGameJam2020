using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassOffset : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().material.SetFloat("_ExtrusionAmount",  Random.Range(17f, 19f));
    }
}
