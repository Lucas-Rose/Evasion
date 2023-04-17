using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    [Header ("Dispenser Settings")]
    [SerializeField] private float maxProjectileCount;
    [SerializeField] private GameObject spawnPointContainer;

    [Header ("Projectile Settings")]
    [SerializeField] private float maxProjSize;
    [SerializeField] private float minProjSize;
    [SerializeField] private GameObject timeProjectilePrefab;
    [SerializeField] private GameObject musicProjectilePrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float trackAccuracyDamp;

    [Header("Custom Dispense Sequence")]
    [SerializeField] private Vector4[] projectileEventInfo;
    private List<ProjectileEvent> projectileEvents = new List<ProjectileEvent>();
    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<float> projectileTimes = new List<float>();

    private float currentTime;
    private Transform projectileContainer;
    void Start()
    {
        playingMusic = true;
        projectileContainer = transform.GetChild(1);
        timeBetweenBeats = 1 / (bpm / 60f);
        musicTime = timeBetweenBeats;
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

        musicTime -= Time.deltaTime;
        if (musicTime <= 0 && playingMusic)
        {
            SpawnProjectile(false, beatProjSpeed, Random.Range(0,2) == 0, Random.Range(0, spawnPoints.Count));
            musicTime = timeBetweenBeats;
        }

        if(musicCalendar.Count > 0)
        {
            if(currentTime > musicCalendar[0].x)
            {
                parseMusicInfo(musicCalendar[0].y);
                musicCalendar.RemoveAt(0);
            }
        }

        if (projectileEvents.Count > 0)
        { 
            if (currentTime > projectileEvents[0].getTime())
            {
                SpawnProjectile(true, projectileEvents[0].getSpeed(), projectileEvents[0].getTracking(), projectileEvents[0].getCannon());
            }
        }
    }

    public void SpawnProjectile(bool custom, float speed, bool tracking, int cannon)
    {
        if(cannon > spawnPoints.Count)
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
        Rigidbody rb = newProj.GetComponent<Rigidbody>();
        Vector3 dir;
        if(!tracking)
        {
            dir = Vector3.back * speed;
        }
        else
        {
            Vector3 dampTarget = new Vector3(playerTransform.position.x + Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
                playerTransform.position.y + Random.Range(-trackAccuracyDamp, trackAccuracyDamp),
                playerTransform.position.z + Random.Range(-trackAccuracyDamp, trackAccuracyDamp));
            dir = (dampTarget - newProj.transform.position).normalized * speed;
        }
        rb.useGravity = false;
        rb.velocity = dir;
        rb.angularVelocity = new Vector3(100f/Random.Range(1, 100), 100f/Random.Range(1, 100), 100f/Random.Range(1, 100));

        if (custom)
        {
            projectileTimes.RemoveAt(0);
            projectileEvents.RemoveAt(0);
        }
        if(projectileContainer.transform.childCount > maxProjectileCount)
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
        for(int i = 0; i < projectileEvents.Count; i++)
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
        if(val == 2)
        {
            SetHalfTime(true);
            Debug.Log("halftime on");
        }
        if(val == 3)
        {
            SetHalfTime(false);
            Debug.Log("halfTime off");
        }
    }
    public void ToggleMusic(bool state)
    {
        playingMusic = state;
    }
}
