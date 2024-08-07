using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxBehavior : MonoBehaviour
{
    public float xm;
    public float xM;
    public float zm;
    public float zM;
    public AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        InvokeRepeating("Spawn", Random.Range(20f, 40f), Random.Range(30f, 40f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            sound.Play();
        }

    }

        private void Spawn()
    {
        if (Timer.enable == true)
        {
            this.gameObject.SetActive(true);
            Vector3 newPos = new Vector3(Random.Range(xm, xM), 0, Random.Range(zm, zM));
            this.transform.position = newPos;
        }
    }
}
