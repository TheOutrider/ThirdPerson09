// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RifleBullet : MonoBehaviour
// {
//     [SerializeField] private float timeToDestroy;
//     private AudioSource audioSource;
//     float timer;

//     void Start()
//     {
//         audioSource = GetComponent<AudioSource>();
//         audioSource.Play();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         timer += Time.deltaTime;
//         if(timer > timeToDestroy) Destroy(this.gameObject);
//     }

//     private void OnCollisionEnter(Collision other) {
//          Destroy(this.gameObject);
//     }
// }
