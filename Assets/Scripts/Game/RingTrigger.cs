using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTrigger : MonoBehaviour
{
    private GameObject[] dogs;

    // Start is called before the first frame update
    void Start()
    {
        dogs = GameObject.FindGameObjectsWithTag("CelluloDogs");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnTriggerEnter(Collider other) { 
        Debug.Log(other.transform.parent.gameObject.name + " triggers.");

        if (other.transform.parent.gameObject.CompareTag("Sheep") && other.transform.parent.GetComponent< GhostSheepBehavior>().mode)
        {
            if ((dogs[0].transform.position - other.transform.position).magnitude <
                (dogs[1].transform.position - other.transform.position).magnitude)
            {
                dogs[0].GetComponent<Score>().IncreaseScore(1, 0);
            }
            else
            {
                dogs[1].GetComponent<Score>().IncreaseScore(1, 0);
            }
        }
        
           
}
    }
