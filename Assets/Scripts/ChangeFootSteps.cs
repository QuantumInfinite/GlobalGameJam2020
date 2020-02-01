using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ChangeFootSteps : MonoBehaviour
{
    AudioClip[] m_FootstepSounds;
    AudioClip[] oldFootsteps;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<FirstPersonController>();
        if(player)
        {
            oldFootsteps = player.m_FootstepSounds;
            player.m_FootstepSounds = m_FootstepSounds;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<FirstPersonController>();
        if (player)
        {
            player.m_FootstepSounds = oldFootsteps;
        }
    }
}