using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public AudioSource winPointAudio;
    public AudioSource loosePointAudio;
    public AudioSource GemUsedAudio;
    public TMP_Text score;

    private int i = 0;

    /// <summary>
    ///   increases the score with the points,
    ///   type 0 is when the sheep enters the ring,
    ///   type 1 is when the gem is used,
    /// </summary>
    public void IncreaseScore(int points, int type)
    {
            winPointAudio.Play();
            i += points;
    }

    /// <summary>
    ///   increases the score with the points,
    ///   type 0 is when the ghost touches the player,
    ///   type 1 is when the gem is used 
    /// </summary>
    public void DecreaseSecore(int points, int type)
    {        
            i -= points;
    }

    public int GetScore()
    {
        return i;
    }

    // Start is called before the first frame update
    void Start()
    {
        score.text = string.Format("{0:00}", i);
    }

    // Update is called once per frame
    void Update()
    {
        score.text = string.Format("{0:00}", i);
    }
}
