using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


//Input Keys
public enum InputKeyboard
{
    arrows = 0,
    wasd = 1,
    mouse = 2,
    computer = 3
}
public class PlayerBehaviour : AgentBehaviour
{
    public TMP_Text otherPlayerScore;
    private bool clicked = false;
    private bool freeze = false;
    private float freezeTime;
    private int inverted = 1;
    private float invertTime;
    private bool CelluloStarted = false;

    public InputKeyboard inputKeyboard;

    public TMP_Dropdown dropColor;
    public TMP_Dropdown dropColorOther;
    public TMP_Dropdown dropMov;
    public TMP_Dropdown dropMovOther;
    public TMP_Text score;
    public GameObject settingMenu;
    public GameObject pauseMenu;
    public GameObject ball;
    public GameObject otherPlayer;

    public void Start()
    {
        agent.SetCasualBackdriveAssistEnabled(true);
        agent.MoveOnIce();
    }

    private void Update()
    {
        if ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).magnitude < 14.7f && Input.GetMouseButton(0))
        {
            clicked = true;
        }

        if (Input.GetMouseButton(0) == false)
        {
            clicked = false;
        }

        if(freeze)
        {
            if (freezeTime > 0)
            {
                freezeTime -= Time.deltaTime;
            }else
            {
                freeze = false;
                agent.MoveOnIce();
            }
        }

        if (inverted == -1)
        {
            if (invertTime > 0)
            {
                invertTime -= Time.deltaTime;
            }
            else
            {
                inverted = 1;
                agent.MoveOnIce();
            }

        }
    }

    public bool IsCelluloStarted()
    {
        return CelluloStarted;
    }

    override public void OnCelluloLongTouch(int key)
    {
        CelluloStarted = true;
        if (CelluloStarted && otherPlayer.GetComponent<PlayerBehaviour>().IsCelluloStarted())
        {
            Timer.enable = true;
            agent.MoveOnIce();
            settingMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
    }

    public void ColorSelector()
    {
        switch (dropColor.options[dropColor.value].text)
        {
            case "RED":
                agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.red, 100);
                score.color = Color.red;
                break;
            case "BLUE":
                agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 100);
                score.color = Color.blue;
                break;
            case "CYAN":
                agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.cyan, 100);
                score.color = Color.cyan;
                break;
            default: break;
        }
        for (int i = 0; i < dropColorOther.options.Count; i++)
        {
            if (dropColorOther.options[i].text == dropColor.options[dropColor.value].text)
            {
                dropColorOther.options.Remove(dropColorOther.options[i]);
            }
        }
        for (int i = 0; i < dropColor.options.Count; i++)
        {
            if (dropColor.options[i].text != dropColor.options[dropColor.value].text)
            {
                dropColorOther.options.Add(dropColor.options[i]);
            }
        }

    }

    public void MovementSelector()
    {
        switch (dropMov.options[dropMov.value].text)
        {
            case "ARROWS":
                inputKeyboard = InputKeyboard.arrows;
                break;
            case "WASD":
                inputKeyboard = InputKeyboard.wasd;
                break;
            case "MOUSE":
                inputKeyboard = InputKeyboard.mouse;
                break;
            case "COMPUTER":
                inputKeyboard = InputKeyboard.computer;
                break;
            default:
                break;
        }

        for (int i = 0; i < dropMovOther.options.Count; i++)
        {
            if (dropMovOther.options[i].text == dropMov.options[dropMov.value].text)
            {
                dropMovOther.options.Remove(dropMovOther.options[i]);
            }
        }

        for (int i = 0; i < dropMov.options.Count; i++)
        {
            if (dropMov.options[i].text != dropMov.options[dropMov.value].text)
            {
                bool test = true;
                for (int j = 0; j < dropMovOther.options.Count; j++)
                {
                    if (dropMovOther.options[j].text == dropMov.options[i].text)
                        test = false;
                }
                if (test)
                    dropMovOther.options.Add(dropMov.options[i]);

            }
        }

    }

    public void freezePlayer()
    {
        freeze = true;
        freezeTime = 5;
        this.agent.SetHapticBackdriveAssist(-1, -1, -1);
    }

    public void invertDirection()
    {
        inverted = -1;
        invertTime = 10;
        this.agent.MoveOnStone();
    }


    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        if (Timer.enable && SceneManager.GetActiveScene().name == "Rehab1" && !freeze)
        {
            //implement your code here
            if (inputKeyboard == InputKeyboard.arrows)
            {
                float vertical = Input.GetAxis("Vertical");

                steering.linear = new Vector3(0f, 0f, inverted * vertical) * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

            }
            else if (inputKeyboard == InputKeyboard.wasd)
            {
                float verticalWasd = Input.GetAxis("VerticalWasd");

                steering.linear = new Vector3(0f, 0f, inverted * verticalWasd) * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

            }
            else if (inputKeyboard == InputKeyboard.mouse)
            {
                if (Input.GetMouseButton(0) && clicked)
                {
                    Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
                    direction /= direction.magnitude;

                    steering.linear = inverted * direction * agent.maxAccel;
                    steering.linear.x = 0f;
                    steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
                }
            }
            else
            {
                Vector3 direction = new Vector3(0, 0, 0);
                direction.z = ball.transform.position.z - this.transform.position.z;
                direction = direction.normalized;
                steering.linear = direction * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
            }
        }
        else if (Timer.enable && SceneManager.GetActiveScene().name == "Rehab2" && !freeze)
        {
            if (inputKeyboard == InputKeyboard.arrows)
            {
                float horizontal = Input.GetAxis("Horizontal");

                steering.linear = new Vector3(inverted * horizontal, 0f, 0f) * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
            }
            else if (inputKeyboard == InputKeyboard.wasd)
            {
                float horizontalWasd = Input.GetAxis("HorizontalWasd");

                steering.linear = new Vector3(inverted * horizontalWasd, 0f, 0f) * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

            }
            else if (inputKeyboard == InputKeyboard.mouse)
            {
                if (Input.GetMouseButton(0) && clicked)
                {
                    Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
                    direction /= direction.magnitude;

                    steering.linear = inverted * direction * agent.maxAccel;
                    steering.linear.y = 0f;
                    steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
                }
            }
            else
            {
                Vector3 direction = new Vector3(0, 0, 0);
                direction.x = ball.transform.position.x - this.transform.position.x;
                direction = direction.normalized;
                steering.linear = direction * agent.maxAccel;
                steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
            }

        }
        return steering;
    }

}
