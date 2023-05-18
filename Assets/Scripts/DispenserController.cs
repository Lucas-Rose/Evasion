using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DispenserController : MonoBehaviour
{ 
    [Header ("Dispenser Settings")]
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private GameObject spawnPointContainer;
    [SerializeField] private float seatedProjectileSize;
    [SerializeField] private float standingProjectileSize;

    [Header("Spread Settings")]
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float seatedSpreadWidth;
    [SerializeField] private float seatedSpreadHeight;
    [SerializeField] private float standingSpreadWidth;
    [SerializeField] private float standingSpreadHeight;
    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [Header("Projectile Settings")]
    private Transform playerTransform;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float trackAccuracyDamp;
    [SerializeField] public float projectileSpeed;
    public float section1Speed;
    public float section2Speed;
    public float section3Speed;
    public float section4Speed;
    public float section5Speed;
    public float section6Speed;

    [Header("Custom Dispense Sequence")]
    private List<GameObject> spawnPoints = new List<GameObject>();

    private Transform projectileContainer;
    private GameManager gManager;
    private bool seated;

    //private GameObject playerHead;
    //[SerializeField] private float startElevation;
    
   

    void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().Play();
        projectileContainer = transform.GetChild(1); //Setting the transform for spawnpoints to instantiated within

        playerTransform = GameObject.Find("PlayerHitbox").GetComponent<Transform>();
        //playerHead = GameObject.Find("CenterEye");
        



        gManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        seated = gManager.getSeated(); //Set via Canvas Interaction

        projectileSpeed = section1Speed;

        //startElevation = playerHead.transform.position.y;
        //gameObject.transform.position = new Vector3(0, startElevation - 0f, 15f);


        GenerateSpawnPoints();

        //TopRowTracking();
    }
    public void GenerateSpawnPoints()
    {


        Vector3 container = spawnPointContainer.transform.position;
        if (seated)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    spawnPoints.Add(Instantiate(spawnPoint, new Vector3(container.x + (-columns / 2) * seatedSpreadWidth + (j * seatedSpreadWidth),
                                                                            container.y + (seatedSpreadHeight / 2) + (i * seatedSpreadHeight), 
                                                                                container.z), Quaternion.identity, spawnPointContainer.transform));
                }
            }
        }
        else
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                    spawnPoints.Add(Instantiate(spawnPoint, new Vector3(container.x + (-columns / 2) * standingSpreadWidth + (j * standingSpreadWidth),
                                                                            container.y + 1 + (standingSpreadHeight / 2) + (i * standingSpreadHeight), 
                                                                                container.z), Quaternion.identity, spawnPointContainer.transform));
                }
            }
        }
    }

    public void SingleTracking(int cannon)
    {
        SpawnProjectile(cannon, true);
    }
    public void SingleStraight(int cannon)
    {
        SpawnProjectile(cannon, false);
    }

    public void BottomRowStraight()
    {
        for(int i = 0; i < columns; i++)
        {
            SpawnProjectile(i, false);
        }
    }

    public void TopRowStraight()
    {
        for (int i = rows*columns-1; i >= rows * columns - columns; i--)
        {
            SpawnProjectile(i, false);
        }
    }

    public void TopRowTracking()
    {
        for (int i = rows * columns - 1; i >= rows * columns - columns; i--)
        {
            SpawnProjectile(i, true);
        }
    }

    public void BottomRowTracking()
    {
        for (int i = 0; i < columns; i++)
        {
            SpawnProjectile(i, true);
        }
    }
    
    public void RandomSingleTracking()
    {
        int cannon = UnityEngine.Random.Range(0, spawnPoints.Count);
        SpawnProjectile(cannon, true);
    }

    public void RandomSingleStraight()
    {
        int cannon = UnityEngine.Random.Range(0, spawnPoints.Count);
        SpawnProjectile(cannon, false);
    }

    //Randomly selects 'amount' of spawnpoints and produces tracking/straight projectiles
    public void BunchTracking(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            int cannonNumber = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnProjectile(cannonNumber, true);
        }
    }
    public void BunchStraight(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int cannonNumber = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpawnProjectile(cannonNumber, false);
        }
    }

    public void SpawnProjectile(int cannon, bool tracking)
    {
        if (cannon > spawnPoints.Count)
        {
            cannon = spawnPoints.Count - 1;
        }


        //Make Projectile
        GameObject prefab = projectiles[UnityEngine.Random.Range(0, projectiles.Length)];
        GameObject newProj = Instantiate(prefab, spawnPoints[cannon].transform.position, Quaternion.identity, projectileContainer);

        //Scale
        if (seated)
        {
            newProj.transform.localScale = new Vector3(seatedProjectileSize, seatedProjectileSize, seatedProjectileSize);
        }
        else
        {
            newProj.transform.localScale = new Vector3(standingProjectileSize, standingProjectileSize, standingProjectileSize);
        }

        //Give Direction
        Rigidbody rb = newProj.GetComponent<Rigidbody>();
        Vector3 dir;
        if (!tracking)
        {
            dir = Vector3.back * projectileSpeed;
        }
        else
        {
            Vector3 dampTarget = new Vector3(playerTransform.position.x + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
                                                playerTransform.position.y + UnityEngine.Random.Range(-trackAccuracyDamp -.5f, trackAccuracyDamp -.5f),
                                                    playerTransform.position.z + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp));

            dir = (dampTarget - newProj.transform.position).normalized * projectileSpeed;
        }
        rb.useGravity = false;
        rb.velocity = dir;

        //Random Rotation
        rb.angularVelocity = new Vector3(100f / UnityEngine.Random.Range(1, 100), 100f / UnityEngine.Random.Range(1, 100), 100f / UnityEngine.Random.Range(1, 100));

        //Cleanup
        if (projectileContainer.transform.childCount > maxProjectileCount)
        {
            Destroy(projectileContainer.transform.GetChild(0).gameObject);
        }
    }
}
