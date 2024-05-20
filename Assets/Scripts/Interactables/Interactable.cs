using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    private bool isDoorOpen;

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        isDoorOpen = !isDoorOpen;
        // No code written here
        // This is a template function to be override by the subclass
    }
}
