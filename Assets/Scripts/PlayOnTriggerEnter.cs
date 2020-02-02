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
    public bool MoveToStartLocation;
    private void OnTriggerEnter(Collider other)
    {
        var grab = other.GetComponent<Grabbable>();
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
        else if(grab && MoveToStartLocation)
        {
            GameManager.instance.OnExitPlayZone(grab);
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
