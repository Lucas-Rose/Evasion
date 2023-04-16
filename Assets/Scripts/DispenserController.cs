using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserController : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float maxProjSize;
    [SerializeField] private float minProjSize;
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private GameObject spawnPointContainer;
    [SerializeField] private float trackAccuracyDamp;
    [SerializeField] private Vector3[] projectileEventInfo;

    private List<ProjectileEvent> projectileEvents = new List<ProjectileEvent>();
    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> activeProjectiles = new List<GameObject>();
    private List<float> projectileTimes = new List<float>();
    private float currentTime;
    void Start()
    {
        for(int i = 0; i < spawnPointContainer.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointContainer.transform.GetChild(i).gameObject);
        }
        CreateProjectileSequence();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(projectileEvents.Count > 0)
        { 
            if (currentTime > projectileEvents[0].getTime())
            {
                SpawnProjectile();
            }
        }
       
    }

    public void SpawnProjectile()
    {
        GameObject newProj = Instantiate(projectilePrefab, spawnPoints[projectileEvents[0].getCannon()].transform.position, Quaternion.identity);
        Rigidbody rb = newProj.GetComponent<Rigidbody>();
        Vector3 dir;
        if(projectileEvents[0].getTracking() == 0)
        {
            dir = Vector3.back * projectileSpeed;
        }
        else
        {
            Vector3 dampTarget = new Vector3(playerTransform.position.x + Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
                playerTransform.position.y + Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
                playerTransform.position.z + Random.Range(-trackAccuracyDamp, trackAccuracyDamp));
            dir = (dampTarget - newProj.transform.position).normalized * projectileSpeed;
        }
        rb.useGravity = false;
        rb.velocity = dir;
        rb.angularVelocity = new Vector3(100f/Random.Range(1, 100), 100f/Random.Range(1, 100), 100f/Random.Range(1, 100));
        activeProjectiles.Add(newProj);
        projectileTimes.RemoveAt(0);
        projectileEvents.RemoveAt(0);


        if (activeProjectiles.Count > maxProjectileCount)
        {
            Destroy(activeProjectiles[0]);
            activeProjectiles.RemoveAt(0);
        }
    }

    public void CreateProjectileSequence()
    {
        for (int i = 0; i < projectileEventInfo.Length; i++)
        {
            //x = time in sequence, y = directed at player? (0 = no, 1 = yes), z = spawnPoint identifier)
            projectileEvents.Add(ScriptableObject.CreateInstance<ProjectileEvent>().init(projectileEventInfo[i].x, projectileEventInfo[i].y, projectileEventInfo[i].z));
        }
        for(int i = 0; i < projectileEvents.Count; i++)
        {
            projectileTimes.Add(projectileEvents[i].getTime());
        }
    }
}
