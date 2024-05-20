using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    public bool letPlay = false, reachedLocation = false;

    public GameObject energyPrefab, zombiePrefab, giantPrefab, alienPrefab, bossPrefab;
    public GameObject player;
    public int zombieCount, giantCount, alienCount, bossCount;

    private GameObject energyBlob, energyAura, energyParticles;
    private AudioSource audioSource;

    [SerializeField]
    private float minimumDistance = 20;


    void Start()
    {
        GameObject originalGameObject = Instantiate(energyPrefab);
        originalGameObject.transform.position = transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        //To find `child1` which is the first index(0)
        energyBlob = originalGameObject.transform.GetChild(0).gameObject;

        //To find `child2` which is the second index(1)
        energyAura = originalGameObject.transform.GetChild(1).gameObject;

        //To find `child3` which is the third index(2)
        energyParticles = originalGameObject.transform.GetChild(2).gameObject;


        energyBlob.GetComponent<ParticleSystem>().Stop();
        energyAura.GetComponent<ParticleSystem>().Stop();
        energyParticles.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {

        if (!reachedLocation)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < minimumDistance)
            {
                reachedLocation = true;
                letPlay = true;
            }
        }
        if (letPlay)
        {
            letPlay = false;
            energyBlob.GetComponent<ParticleSystem>().Play();
            energyAura.GetComponent<ParticleSystem>().Play();
            energyParticles.GetComponent<ParticleSystem>().Play();
            audioSource.Play();
            StartCoroutine(StopEnergy());
        }
    }

    IEnumerator StopEnergy()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            yield return new WaitForSeconds(2);
            Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        }

        for (int i = 0; i < giantCount; i++)
        {
            yield return new WaitForSeconds(3);
            Instantiate(giantPrefab, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < alienCount; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(alienPrefab, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < bossCount; i++)
        {
            yield return new WaitForSeconds(3);
            Instantiate(bossPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1);
        letPlay = false;
        audioSource.Stop();
        energyBlob.GetComponent<ParticleSystem>().Stop();
        energyAura.GetComponent<ParticleSystem>().Stop();
        energyParticles.GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);
    }

    public void TrustOn()
    {
        letPlay = true;
    }

    public void TrustOff()
    {
        letPlay = false;
    }
}
