using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnTriggerEnter : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem ps;
    public Animator anim;
    public string animTrigger;
    public GameObject myGameObject;
    public bool turnOffObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(myGameObject)
            {
                gameObject.SetActive(turnOffObject);
            }
            if (audioSource)
            {
                audioSource.Play();
            }
            if(ps)
            {
                ps.Play();
            }
            if(anim)
            {
                anim.SetTrigger(animTrigger);
            }
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (audioSource && audioSource.loop)
            {
                audioSource.Stop();
            }
        }
    }
}
