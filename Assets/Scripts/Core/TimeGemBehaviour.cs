using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGemBehaviour : MonoBehaviour
{
    public AudioSource appearSound;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        InvokeRepeating("Spawn", Random.Range(60f, 70f), Random.Range(40f, 50f));
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