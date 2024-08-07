using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGemBehaviour : MonoBehaviour
{
    public AudioSource appearSound;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        InvokeRepeating("Spawn", Random.Range(30f, 40f), Random.Range(30f, 40f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {
        if (Timer.enable == true)
        {
            this.gameObject.SetActive(true);
            appearSound.Play();
            Vector3 newPos = new Vector3(Random.Range(0.64f, 13.64f), 0, Random.Range(-0.5f, -9.49f));
            this.transform.position = newPos;
        }
    }
}

