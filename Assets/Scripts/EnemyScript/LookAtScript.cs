using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{

    private Transform cam;
    [SerializeField]
    private Transform target;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (target == null)
            transform.LookAt(cam);
        else
            transform.LookAt(target);
    }
}
