using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelConsole : Interactable
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Interact()
    {
        PlayerPrefs.SetInt("TunnelDoor", 1);
        audioSource.PlayOneShot(audioSource.clip);
        promptMessage = "Tunnel Systems Activated";
    }
}
