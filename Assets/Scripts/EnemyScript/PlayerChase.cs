// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UI;

// public class PlayerChase : MonoBehaviour
// {

//     public GameObject player;
//     private NavMeshAgent agent;
//     private Animator animator;
//     [SerializeField]
//     private int health = 100, maxHealth = 100;
//     private Rigidbody rb;
//     [SerializeField]
//     private Slider healthBar;
//     bool isAttacking = false;
//     bool isMeleeAttacking = false;
//     public float attackForce = 200.0f;

//     private PlayerController playerController;
//     [SerializeField]
//     private GameObject explosion;

//     bool enemySpawned;


//     private BoxCollider handCollider;

//     private void OnCollisionEnter(Collision other)
//     {

//         if (other.gameObject.tag == "RifleBullet" && health > 0)
//             health -= 1;
//     }

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         agent = GetComponent<NavMeshAgent>();
//         rb = GetComponent<Rigidbody>();
//         playerController = new PlayerController();
//         StartCoroutine(Initiate());
//         handCollider = GetComponentInChildren<BoxCollider>();
//     }

//     IEnumerator Initiate()
//     {
//         yield return new WaitForSeconds(3);
//         animator.SetBool("Spawned", true);
//         enemySpawned = true;
//     }

//     void Update()
//     {
//         if (health > 0)
//         {
//             if (enemySpawned)
//             {
//                 agent.SetDestination(player.transform.position + new Vector3(0.0f, 0.0f, 0.5f));
//                 if (agent.velocity.magnitude == 0)
//                 {
//                     animator.SetBool("Walk", false);
//                 }
//                 else
//                 {
//                     if (!isAttacking)
//                     {
//                         animator.SetBool("Walk", true);
//                     }
//                 }

//                 if (Vector3.Distance(player.transform.position, transform.position) < 3)
//                 {
//                     if (isAttacking == false)
//                     {
//                         isAttacking = true;
//                         animator.SetBool("Hit", true);
//                         StartCoroutine(ResetAttack());
//                     }
//                 }
//                 UpdateHealthBar(health, maxHealth);
//             }
//         }
//         else
//         {
//             agent.SetDestination(agent.transform.position);
//             animator.SetBool("Dying", true);
//             rb.constraints = RigidbodyConstraints.FreezeRotation;
//             healthBar.gameObject.SetActive(false);

//         }

//     }

//     private void UpdateHealthBar(float currentValue, float maxValue)
//     {
//         healthBar.value = currentValue / maxValue;
//     }



//     IEnumerator ResetAttack()
//     {

//         yield return new WaitForSeconds(2);
//         explosion.gameObject.SetActive(true);
//         isAttacking = false;
//         animator.SetBool("Hit", false);
//         animator.SetBool("Walk", false);
//         StartCoroutine(StopExplosion());
//         ExertDamage();
//     }

//     IEnumerator StopExplosion()
//     {
//         yield return new WaitForSeconds(1);
//         explosion.gameObject.SetActive(false);
//     }

//     private void ExertDamage()
//     {
//         Collider[] hitObjects = Physics.OverlapSphere(transform.position, 7.5f);

//         for (int i = 0; i < hitObjects.Length; i++)
//         {
//             CapsuleCollider capsuleCollider = hitObjects[i].GetComponent<CapsuleCollider>() ?? null;
//             if (capsuleCollider != null)
//             {
//                 if (capsuleCollider.gameObject.tag == "Player")
//                 {
//                     PlayerController playerController = hitObjects[i].gameObject.GetComponent<PlayerController>();
//                     playerController.HandlePlayerHealth(1);


//                 }
//             }
//         }
//     }
// }
