using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorConsole : Interactable
{

    [SerializeField]
    private GameObject tunnelDoor;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip defaultClip, errorClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    protected override void Interact()
    {
        // base.Interact();
        if (PlayerPrefs.GetInt("TunnelDoor") == 1)
        {
            tunnelDoor.GetComponent<Animator>().SetBool("isOpen", true);
            audioSource.PlayOneShot(defaultClip);
        }
        else
        {
            audioSource.PlayOneShot(errorClip);
            promptMessage = "Activate tunnel systems to open the door";
        }

    }
}
