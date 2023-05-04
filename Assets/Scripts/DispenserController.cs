using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DispenserController : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private float bpm;
    [SerializeField] private float beatProjSpeed;
    [SerializeField] private List<Vector2> musicCalendar;
    private bool playingMusic;
    private float timeBetweenBeats;
    private float musicTime;
    private bool halfTime;
    public bool isSeated; //determined by player's initial input
    public float widthSpacing; //tbd
    public float heightSpacing; //tbd

    [Header ("Dispenser Settings")]
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private GameObject spawnPointContainer;
    [SerializeField] private float seatedProjectileSize;
    [SerializeField] private float standingProjectileSize;

    [Header("Spread Settings")]
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private int seatedSpreadWidth;
    [SerializeField] private int seatedSpreadHeight;
    [SerializeField] private int standingSpreadWidth;
    [SerializeField] private int standingSpreadHeight;
    [SerializeField] private int columns;
    [SerializeField] private int rows;


    [Header("Projectile Settings")]
    [SerializeField] private GameObject timeProjectilePrefab;
    [SerializeField] private GameObject musicProjectilePrefab;
    private Transform playerTransform;
    [SerializeField] private float trackAccuracyDamp;

    [Header("Custom Dispense Sequence")]
    [SerializeField] private Vector4[] projectileEventInfo;
    private List<ProjectileEvent> projectileEvents = new List<ProjectileEvent>();
    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<float> projectileTimes = new List<float>();
    [SerializeField] private bool dispensable;

    private float currentTime;
    private Transform projectileContainer;
    private GameManager gManager;
    [SerializeField] private bool seated;
    void Start()
    {
        Debug.Log(seated);
        projectileContainer = transform.GetChild(1); //Setting the transform for spawnpoints to instantiated within

        playingMusic = true; //Handling the rewind mechanic
        timeBetweenBeats = 1 / (bpm / 60f); //Music-based dispensing
        musicTime = timeBetweenBeats;

        gManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        seated = gManager.getSeated(); //Set via Canvas Interaction
        Vector3 container = spawnPointContainer.transform.position;
        if (seated)
        {
            for (int i = 0; i < rows; i ++)
            {
                for (int j = 0; j < columns; j ++)
                {
                    spawnPoints.Add(Instantiate(spawnPoint, new Vector3((-columns/2)*seatedSpreadWidth + (j * seatedSpreadWidth), (seatedSpreadHeight / 2) + (i * seatedSpreadHeight), container.z), Quaternion.identity, spawnPointContainer.transform));
                }
            }
        }
        else
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    
                    spawnPoints.Add(Instantiate(spawnPoint, new Vector3((-columns / 2)*standingSpreadWidth + (j * standingSpreadWidth), 1 + (standingSpreadHeight / 2) + (i * standingSpreadHeight), container.z), Quaternion.identity, spawnPointContainer.transform));
                }
            }
        }
        CreateProjectileSequence();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(playerTransform == null)
        {
            try
            {
                playerTransform = GameObject.Find("Head").GetComponent<Transform>();
            }
            catch(Exception e) 
            {
                Debug.Log("Head hasn't loaded yet");
            }
        }
        musicTime -= Time.deltaTime;
        if (musicTime <= 0 && playingMusic)
        {
            SpawnProjectile(false, beatProjSpeed, UnityEngine.Random.Range(0, 2) == 0, UnityEngine.Random.Range(0, spawnPoints.Count));
            musicTime = timeBetweenBeats;
        }

        if (musicCalendar.Count > 0)
        {
            if (currentTime > musicCalendar[0].x)
            {
                parseMusicInfo(musicCalendar[0].y);
                musicCalendar.RemoveAt(0);
            }
        }

        if (projectileEvents.Count > 0 && dispensable)
        {
            if (currentTime > projectileEvents[0].getTime())
            {
                SpawnProjectile(true, projectileEvents[0].getSpeed(), projectileEvents[0].getTracking(), projectileEvents[0].getCannon());
            }
        }
    }

    public void SpawnProjectile(bool custom, float speed, bool tracking, int cannon)
    {
        if (cannon > spawnPoints.Count)
        {
            cannon = spawnPoints.Count - 1;
        }
        GameObject newProj = null;

        if (custom)
        {
            newProj = Instantiate(timeProjectilePrefab, spawnPoints[cannon].transform.position, Quaternion.identity, projectileContainer);
        }
        else
        {
            newProj = Instantiate(musicProjectilePrefab, spawnPoints[cannon].transform.position, Quaternion.identity, projectileContainer);
        }

        if (seated)
        {
            newProj.transform.localScale = new Vector3(seatedProjectileSize, seatedProjectileSize, seatedProjectileSize);
            Debug.Log(newProj.transform.localScale.x);
        }
        else
        {
            newProj.transform.localScale = new Vector3(standingProjectileSize, standingProjectileSize, standingProjectileSize);
            Debug.Log(newProj.transform.localScale.x);
        }

        Rigidbody rb = newProj.GetComponent<Rigidbody>();
        Vector3 dir;
        if (!tracking)
        {
            dir = Vector3.back * speed;
        }
        else
        {
            Vector3 dampTarget = new Vector3(playerTransform.position.x + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
            playerTransform.position.y + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
            playerTransform.position.z + UnityEngine.Random.Range(-trackAccuracyDamp, trackAccuracyDamp));
            dir = (dampTarget - newProj.transform.position).normalized * speed;
        }
        rb.useGravity = false;
        rb.velocity = dir;
        rb.angularVelocity = new Vector3(100f / UnityEngine.Random.Range(1, 100), 100f / UnityEngine.Random.Range(1, 100), 100f / UnityEngine.Random.Range(1, 100));

        if (custom)
        {
            projectileTimes.RemoveAt(0);
            projectileEvents.RemoveAt(0);
        }
        if (projectileContainer.transform.childCount > maxProjectileCount)
        {
            Destroy(projectileContainer.transform.GetChild(0).gameObject);
        }
    }

    public void CreateProjectileSequence()
    {
        for (int i = 0; i < projectileEventInfo.Length; i++)
        {
            //x = time in sequence, y = speed, z = tracking ? 0 : 1, z = cannon
            projectileEvents.Add(ScriptableObject.CreateInstance<ProjectileEvent>().init(projectileEventInfo[i].x, projectileEventInfo[i].y, projectileEventInfo[i].z, projectileEventInfo[i].w));
        }
        for (int i = 0; i < projectileEvents.Count; i++)
        {
            projectileTimes.Add(projectileEvents[i].getTime());
        }
    }
    public void SetHalfTime(bool state)
    {
        halfTime = state;
        if (halfTime)
        {
            timeBetweenBeats *= 2;
        }
        else
        {
            timeBetweenBeats /= 2;
        }
        musicTime = 0;
    }
    public void parseMusicInfo(float val)
    {
        if (val == 0)
        {
            ToggleMusic(false);
            Debug.Log("music off");
        }
        if (val == 1)
        {
            ToggleMusic(true);
            Debug.Log("music on");
        }
        if (val == 2)
        {
            SetHalfTime(true);
            Debug.Log("halftime on");
        }
        if (val == 3)
        {
            SetHalfTime(false);
            Debug.Log("halfTime off");
        }
    }
    public void ToggleMusic(bool state)
    {
        playingMusic = state;
    }

    public void SetTime(float val)
    {
        currentTime = val;
    }
    public void SetDispensable(bool state)
    {
        dispensable = state;
    }

}
