using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureParent : MonoBehaviour
{
    public List<ReturnToHome> children;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool returning = false;
    // Update is called once per frame
    void Update()
    {
        if (!returning && Input.GetKeyDown(KeyCode.Equals))
        {
            Debug.Log("Returning");
            returning = true;
            foreach (var c in children)
            {
                c.StartCoroutine("GoHome");
            }
            Invoke("Check", 2f);
        }
    }

    void Check()
    {
        foreach (var c in children)
        {
            if (c.transform.position != c.origionalPos)
            {
                return;
            }
        }
    }
}
