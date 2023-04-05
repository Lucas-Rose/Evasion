using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float dispenseTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float maxProjSize;
    [SerializeField] private float minProjSize;
    [SerializeField] private float maxProjectileCount;
    //band-aid fix
    [SerializeField] private float currentProjectileCount;
    public GameObject self;


    private List<GameObject> activeProjectiles;
    private float currentTime;

    private BoxCollider col; //used to determine the size of the area in which projectiles can spawn
    // Start is called before the first frame update
    void Start()
    {
        currentTime = dispenseTime;
        col = GetComponent<BoxCollider>();
        activeProjectiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            //Debug.Log("Dispensing");
            SpawnProjectile();
            currentTime = dispenseTime;
        }
    }

    public void SpawnProjectile()
    {
        float xPos = Random.Range(transform.position.x - (col.size.x / 2), transform.position.x + (col.size.x / 2));
        float yPos = Random.Range(transform.position.y - (col.size.y / 2), transform.position.y + (col.size.y / 2));
        Quaternion randomRotation = Random.rotation;
        GameObject newProj = Instantiate(projectilePrefab, new Vector3(xPos, yPos, transform.position.z), randomRotation, gameObject.transform);
        float projSize = Random.Range(minProjSize, maxProjSize);
        newProj.transform.localScale = new Vector3(projSize, projSize, projSize);
        newProj.GetComponent<Rigidbody>().velocity = -Vector3.forward * projectileSpeed;
        activeProjectiles.Add(newProj);
        currentProjectileCount += 1;
        if (currentProjectileCount >= maxProjectileCount) //currentProjectileCount instead of.count
        {
            Destroy(transform.GetChild(0).gameObject);
            currentProjectileCount -= 1;
        }
    }

    public void destroyedElsewhere()
    {
        currentProjectileCount -= 1;
    }
}
