using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{

    private Animator animator;
    private GameObject player;
    private PlayerController playerController;
    private AudioSource audioSource;

    private bool isAttacking = false, spawned = false, playerDead = false, dodgeBullet = false;
    private int health = 500, maxHealth = 500;

    [SerializeField]
    private float amplitude = 0.5f, frequency = 1f, dodgeSpeed, dodgeX = 2.5f;

    private Vector3 originalPosition = new Vector3(), tempPosition = new Vector3(), dodgePosition = new Vector3();

    private string[] attacks = new string[] { "Light", "Medium", "Heavy" };
    private int attackIndex = 0;

    [SerializeField] private GameObject magicBullet, fireball, handPosition, magicBulletParent;

    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    private Slider healthBar;

    private Vector3 dir = Vector3.left;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerController = player.gameObject.GetComponent<PlayerController>();
        originalPosition = transform.position;
        StartCoroutine(Initiate());
        audioSource = GetComponent<AudioSource>();

    }

    IEnumerator Initiate()
    {
        ;
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Spawned", true);
        spawned = true;
    }

    void Update()
    {

        if (spawned && health > 0 && !playerDead)
        {
            transform.LookAt(player.transform);
            if (!isAttacking)
            { AttackPlayer(); }


            if (!dodgeBullet)
            {
                FloatInAir();

            }

            else if (!isAttacking)
            {
                dodgePosition = new Vector3(dodgePosition.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, dodgePosition, dodgeSpeed * Time.deltaTime);
                originalPosition = transform.position;

            }
            CheckForBullet();
        }
        UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            audioSource.Stop();
            healthBar.gameObject.SetActive(false);
            playerController.GameCompleted();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword" && health > 0 && spawned)
        {
            health -= 25;
        }

        if (other.gameObject.tag == "RifleBullet" && health > 0 && spawned)
        {
            health -= 10;
        }
    }

    private void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void FloatInAir()
    {
        tempPosition = originalPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPosition;
    }

    private void CheckForBullet()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, 1.5f);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].gameObject.tag == "RifleBullet")
            {
                dodgeX = dodgeX * -1;
                dodgePosition.x = transform.position.x + dodgeX;
                dodgeBullet = true;
                StartCoroutine(Dodge());
            }
        }
    }

    IEnumerator Dodge()
    {
        Debug.Log("DODGING : ");
        yield return new WaitForSeconds(0.25f);
        dodgeBullet = false;
        // transform.Translate(dir * dodgeSpeed * Time.deltaTime);

        // if (transform.position.x <= -4)
        // {
        //     dir = Vector3.right;
        // }
        // else if (transform.position.x >= 4)
        // {
        //     dir = Vector3.left;
        // }
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        attackIndex = Random.Range(0, 2);
        Debug.Log("ATTACK INDEX : " + attackIndex.ToString());
        animator.SetBool(attacks[attackIndex], true);
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f);
        Fire();
        yield return new WaitForSeconds(2.5f);
        animator.SetBool(attacks[attackIndex], false);

    }

    private void Fire()
    {

        if (attackIndex == 0)
        {
            GameObject bullet = GameObject.Instantiate(fireball, handPosition.transform.position, Quaternion.identity, magicBulletParent.transform);
        }

        if (attackIndex == 1)
        {
            GameObject bullet = GameObject.Instantiate(magicBullet, handPosition.transform.position, Quaternion.identity, magicBulletParent.transform);
        }

        StartCoroutine(ResetAttackStance());
    }

    IEnumerator ResetAttackStance()
    {
        yield return new WaitForSeconds(5);
        isAttacking = false;
    }
}
