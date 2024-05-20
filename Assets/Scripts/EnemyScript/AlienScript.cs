using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AlienScript : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;

    private GameObject player;
    private PlayerController playerController;

    private int health = 100, maxHealth = 100;

    private bool spawned = false, playerDead = false, isAttacking = false;
    [SerializeField]
    private Slider healthBar;
    [SerializeField] private float fireRate, enemyBulletSpeed;
    float fireRateTimer;


    [SerializeField] private GameObject alienBullet;
    [SerializeField] private Transform barrelPosition, alienBulletParent, hitTarget;

    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    private GameObject blood;


    void Awake()
    {
        fireRateTimer = fireRate;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerController = player.gameObject.GetComponent<PlayerController>();
        transform.LookAt(player.transform);
        StartCoroutine(Initiate());
        blood.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        if (spawned && health > 0 && !playerDead)
        {
            CheckIfPlayerDead();
            if (!isAttacking)
            {
                agent.SetDestination(player.transform.position);
            }

            if (agent.velocity.magnitude == 0)
            {
                animator.SetBool("Run", false);
                if (Vector3.Distance(player.transform.position, transform.position) < 12)
                {
                    transform.LookAt(player.transform);
                    animator.SetBool("Attack", true);
                    if (ShouldFire()) Fire();
                }
            }
            else
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Run", true);
            }
            UpdateHealthBar(health, maxHealth);

        }
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            healthBar.gameObject.SetActive(false);
        }
    }


    public bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate)
            return false;
        else
            return true;
    }

    public void Fire()
    {
        fireRateTimer = 0;
        GameObject bullet = GameObject.Instantiate(alienBullet, barrelPosition.position, Quaternion.identity, alienBulletParent);
        Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
        bulletRig.velocity = barrelPosition.TransformDirection(GetDirection(Vector3.forward) * enemyBulletSpeed * Time.deltaTime);
        Destroy(bullet, 1f);
    }



    private Vector3 GetDirection(Vector3 direction)
    {
        direction += new Vector3(
            Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
            Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
            Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
        );
        direction.Normalize();
        return direction;
    }


    IEnumerator Initiate()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Spawned", true);
        spawned = true;
    }

    private void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword" && health > 0 && spawned)
        {
            health -= 25;
            StartCoroutine(SplashBlood());
        }

        if (other.gameObject.tag == "RifleBullet" && health > 0 && spawned)
        {
            health -= 15;
            StartCoroutine(SplashBlood());
        }
    }

    IEnumerator SplashBlood()
    {
        blood.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.10f);
        blood.GetComponent<ParticleSystem>().Stop();
    }

    private void CheckIfPlayerDead()
    {
        if (playerController.playerHealth <= 0)
        {
            playerDead = true;
            isAttacking = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            animator.SetBool("Run", false);
        }
    }
}
