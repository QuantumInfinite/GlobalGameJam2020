using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Climbable : MonoBehaviour
{
    FirstPersonController player;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<FirstPersonController>();

        if(player)
        {
            player.isClimbing = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        player = other.GetComponent<FirstPersonController>();

        if (player)
        {
            player.isClimbing = false;
        }
    }
}
