using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallBehaviour : AgentBehaviour
{
    public enum CurrentPlayer
    {
        player1 = 0,
        player2 = 1
    }
    Steering steering = new Steering();
    Vector3 direction ;
    public GameObject player2;
    public GameObject player1;
    System.Random random = new System.Random();
    private CurrentPlayer currentPlayer;
    bool collided = false;
    float deltaTime = 0;
    public AudioSource ping;
    public AudioSource pong;



    // Start is called before the first frame update
    void Start()
    {
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.white, 100);
        agent.MoveOnIce();
        direction = new Vector3((random.Next(0, 2) * 2 - 1) * Random.Range(1f, 2f), 0, (random.Next(0, 2) * 2 -1 )*Random.Range(1f, 2f)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(deltaTime > 0)
        {
            deltaTime -= Time.deltaTime;
        }
        else
        {
            collided = false;
        }
        
    }

    public override Steering GetSteering()
    {
        if (Timer.enable)
        {
            if (!collided)
            {
                const double RADIUS = 2.5;
                if ((player1.transform.position - this.transform.position).magnitude <= RADIUS )
                {
                    if (SceneManager.GetActiveScene().name == "Rehab1" )
                    {
                        direction = Vector3.Reflect(direction, new Vector3(1, 0, 0));
                        direction += new Vector3(0.4f, 0, 0);
                        direction = direction.normalized;
                        direction += 0.5f * player1.GetComponent<Rigidbody>().velocity.normalized;
                        direction = direction.normalized;
                        ping.Play();
                        collided = true;
                        deltaTime = 1.1f;
                        currentPlayer = CurrentPlayer.player1;
                    }
                    else if (SceneManager.GetActiveScene().name == "Rehab2" )
                    {
                        direction = Vector3.Reflect(direction, new Vector3(0, 0, -1));
                        direction += new Vector3(0, 0, -0.4f);
                        direction = direction.normalized;
                        direction += 0.5f * player1.GetComponent<Rigidbody>().velocity.normalized;
                        direction = direction.normalized;
                        ping.Play();
                        collided = true;
                        deltaTime = 1.1f;
                        currentPlayer = CurrentPlayer.player1;
                    }
                }
                else if ((player2.transform.position - this.transform.position).magnitude <= RADIUS)
                {                   
                    if (SceneManager.GetActiveScene().name == "Rehab1" )
                    {
                        direction = Vector3.Reflect(direction, new Vector3(-1, 0, 0));
                        direction += new Vector3(-0.4f, 0, 0);
                        direction = direction.normalized;
                        direction += 0.5f * player2.GetComponent<Rigidbody>().velocity.normalized;
                        direction = direction.normalized;
                        pong.Play();
                        collided = true;
                        deltaTime = 1.1f;
                        currentPlayer = CurrentPlayer.player2;
                    }
                    else if (SceneManager.GetActiveScene().name == "Rehab2" )
                    {
                        direction = Vector3.Reflect(direction, new Vector3(0, 0, 1));
                        direction += new Vector3(0, 0, 0.4f);
                        direction = direction.normalized;
                        direction += 0.5f * player2.GetComponent<Rigidbody>().velocity.normalized;
                        direction = direction.normalized;
                        pong.Play();
                        collided = true;
                        deltaTime = 1.1f;
                        currentPlayer = CurrentPlayer.player2;
                    }
                }
            }
            steering.linear = direction * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }
        else
        {
            steering.linear = new Vector3(0, 0, 0) * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
        }
        return steering;
    }

    private void OnCollisionEnter(Collision collision)
    {

       if(collision.gameObject.CompareTag("UpperWall"))
        {
            if (SceneManager.GetActiveScene().name == "Rehab1")
            {
                direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            }
            else if (SceneManager.GetActiveScene().name == "Rehab2")
            {
                player2.GetComponent<Score>().IncreaseScore(1, 0);
                agent.SetGoalPosition(7.2f, -5.2f, 3f);
                direction = new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)).normalized;
                player2.GetComponent<CelluloAgent>().SetSimpleVibrate(3f,3f,3f, 1000, 1000);
            }

        }else if (collision.gameObject.CompareTag("LowerWall"))
        {
            if (SceneManager.GetActiveScene().name == "Rehab1")
            {
                direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            }
            else if (SceneManager.GetActiveScene().name == "Rehab2")
            {
                player1.GetComponent<Score>().IncreaseScore(1, 0);
                agent.SetGoalPosition(7.2f, -5.2f, 3f);
                direction = new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)).normalized;
                player1.GetComponent<CelluloAgent>().SetSimpleVibrate(3f, 3f, 3f, 1000, 1000);
            }  
        }else if (collision.gameObject.CompareTag("LeftWall"))
        {
            if (SceneManager.GetActiveScene().name == "Rehab1")
            {
                player2.GetComponent<Score>().IncreaseScore(1, 0);
                agent.SetGoalPosition(7.2f, -5.2f, 3f);
                direction = new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)).normalized;
                player2.GetComponent<CelluloAgent>().SetSimpleVibrate(3f, 3f, 3f, 1000, 1000);
            }
            else if (SceneManager.GetActiveScene().name == "Rehab2")
            {
                direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            }
            
        } else if(collision.gameObject.CompareTag("RightWall"))
        {
            if (SceneManager.GetActiveScene().name == "Rehab1")
            {
                player1.GetComponent<Score>().IncreaseScore(1, 0);
                agent.SetGoalPosition(7.2f, -5.2f, 3);
                direction = new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)).normalized;
                player1.GetComponent<CelluloAgent>().SetSimpleVibrate(3f, 3f, 3f,1000,1000);
            }
            else if (SceneManager.GetActiveScene().name == "Rehab2")
            {
                direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            }
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Snowflake"))
        {
            if(currentPlayer == CurrentPlayer.player1)
            {
                player2.GetComponent<PlayerBehaviour>().freezePlayer();
            }
            else
            {
                player1.GetComponent<PlayerBehaviour>().freezePlayer();
            }
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Star"))
        {
            if (currentPlayer == CurrentPlayer.player1)
            {
                player1.GetComponent<Score>().DecreaseSecore(1, 0);
            }
            else
            {
                player2.GetComponent<Score>().DecreaseSecore(1, 0);
            }
            other.gameObject.SetActive(false);

        }else if (other.gameObject.CompareTag("Gem"))
        {
            direction = new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)).normalized;
            other.gameObject.SetActive(false);
        } else if (other.gameObject.CompareTag("Invertor"))
        {
            if (currentPlayer == CurrentPlayer.player1)
            {
                player2.GetComponent<PlayerBehaviour>().invertDirection();
            }
            else
            {
                player1.GetComponent<PlayerBehaviour>().invertDirection();
            }
            other.gameObject.SetActive(false);
        }
    }


}
