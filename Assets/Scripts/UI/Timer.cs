using UnityEngine;


using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/**
	This class is the implementation of the timer used in the game and how it is handled in it
*/
public class Timer : MonoBehaviour
{
    public GameObject menu;
    public float initTimerValue;
    private TMP_Text timerText;
    public Slider slider;
    public static float maxMinutes = 1;
    public TMP_Text winner;
    public static bool enable;
    public TMP_Text score_2;
    public TMP_Text score_1;
    public TMP_Dropdown drop;

    private Dictionary<Color, string> My_dict =
              new Dictionary<Color, string>(){
                                  {Color.blue, "BLUE"},
                                  {Color.cyan, "CYAN"},
                                {Color.red, "RED"} };


    public void Awake() {
        initTimerValue = 0;
    }

    // Start is called before the first frame update
    public void Start() {
        enable = false;
        slider.value = 0;
        timerText = GetComponent<TMP_Text>();
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }

    public void TimeSelector()
    {
        maxMinutes = int.Parse(drop.options[drop.value].text);

    }

    // Update is called once per frame
    public void Update() {

        //IMPLEEMT YOUR CODE HERE
        if (initTimerValue > maxMinutes*60) {
            enable = false;
            string color;
            if (int.Parse(score_2.text) > int.Parse(score_1.text))
            {
                winner.color = score_2.color;
                My_dict.TryGetValue(score_2.color, out color);
                winner.text = "TEAM " + color;
            }
            else if(int.Parse(score_2.text) < int.Parse(score_1.text))
            {
                winner.color = score_1.color;
                My_dict.TryGetValue(score_1.color, out color);
                winner.text = "TEAM " + color;
            }
            else
            {
                winner.color = Color.white;
                winner.text = "  DRAW  ";
            }
            menu.SetActive(true);
        }
        else
        {
            if (enable)
            {
                initTimerValue += Time.deltaTime;
                timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(initTimerValue / 60), Mathf.FloorToInt(initTimerValue % 60));
                slider.value = initTimerValue / (maxMinutes * 60);
            }
        }
    }
}
