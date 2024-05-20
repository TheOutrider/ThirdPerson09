using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : Interactable
{

    [SerializeField]
    private GameObject tunnelDoor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        // base.Interact();
        Debug.Log("INTERACTED WITH : " + gameObject.name + "");
        tunnelDoor.GetComponent<Animator>().SetBool("isOpen", true);

    }
}
