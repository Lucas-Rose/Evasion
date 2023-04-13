using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleMap : MonoBehaviour
{
    public GameObject jungleTree1;
    public GameObject jungleTree2;
    public GameObject jungleTree3;
    public GameObject environ1;
    public GameObject environ2;
    public int counter = 0;



    public Vector3[] coordPositions;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(jMap());
    }

    public IEnumerator jMap()
    //+0 x is center
    //the counter and array aren't really necessary in this version
    {
        yield return new WaitForSeconds(1);
        Vector3 pos1 = new Vector3(transform.position.x + -1.36f, transform.position.y, transform.position.z);
        Instantiate(jungleTree1, pos1, Quaternion.identity);
        coordPositions[counter] = pos1;
        yield return new WaitForSeconds(1);
        counter++;
        Vector3 pos2 = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
        Instantiate(jungleTree2, pos2, Quaternion.identity);
        coordPositions[counter] = pos2;
        yield return new WaitForSeconds(1.5f);
        counter++;
        Vector3 pos3 = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        Instantiate(jungleTree1, pos3, Quaternion.identity);
        coordPositions[counter] = pos3;
        yield return new WaitForSeconds(.25f);
        Vector3 pos4 = new Vector3(transform.position.x + 0.57f, transform.position.y + 2.66f, transform.position.z);
        Instantiate(environ1, pos4, Quaternion.Euler(new Vector3(0, 0, 34.64f)));
        coordPositions[counter] = pos4;
        counter++;
        Vector3 pos5 = new Vector3(transform.position.x + 3.889944f, transform.position.y + 0.5f, transform.position.z);
        Instantiate(jungleTree3, pos5, Quaternion.identity);
        coordPositions[counter] = pos5;
        counter++;



    }
}
