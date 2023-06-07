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
    [SerializeField] private GameObject block;
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

    private GameObject playerHead;
    [SerializeField] private float startElevation;
    //block/wall stuff
    private Vector3 blockPos;
    private Quaternion blockRot;
   

    void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().Play();
        projectileContainer = transform.GetChild(1); //Setting the transform for spawnpoints to instantiated within

        playerTransform = GameObject.Find("PlayerHitbox").GetComponent<Transform>();
        playerHead = GameObject.Find("CenterEye");
        



        gManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        seated = gManager.getSeated(); //Set via Canvas Interaction

        projectileSpeed = section1Speed;

        startElevation = playerHead.transform.position.y;
        gameObject.transform.position = new Vector3(0, startElevation - 0f, 15f);


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

//Since Unity only allows one input for animation events anything specific requires a custom function to be made here.
//xPos, yPos, xRot, yRot, zRot, xScale, yScale, zScale, speedMod. passing an AnimationEvent function is required to use multiple.
    public void BlockFullWallFlat(float yScale)
    {
        //spawnBlock(0, 0, 0, 0, 0, 20, yScale, 3, 0);
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
                                                playerTransform.position.y + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
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

//different shapes can use a similar method. This one is for 'walls' and big blocks that take up large parts of the screen.
//This is basically a simplified version of the above method with more data entries for specificity. Most can be left empty for most shapes.
//speedMod is a modifier that makes it possible to spawn blocks that are faster or slower than a section's norm by speedMod.
    public void spawnBlock(float xPos, float yPos, float xRot, float yRot, float zRot, float xScale, float yScale, float zScale, float speedMod)
    {
        //to set positions not tied to canons. This is more environmental and at this point not designed to function with seated play.
        blockPos = new Vector3(xPos, yPos, 15);
        //To create shapes such as beams, split angles, etc. Can slant.
        blockRot = Quaternion.Euler(xRot, yRot, zRot);
        GameObject newBlock = Instantiate(block, blockPos, transform.rotation * blockRot);
        newBlock.transform.localScale = new Vector3(xScale, yScale, zScale);

        Rigidbody rb = newBlock.GetComponent<Rigidbody>();
        Vector3 dir;
        Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 30);

        dir = (target - newBlock.transform.position).normalized * (projectileSpeed + speedMod);

        rb.useGravity = false;
        rb.velocity = dir;

        //remember to add clean up
    }
}
