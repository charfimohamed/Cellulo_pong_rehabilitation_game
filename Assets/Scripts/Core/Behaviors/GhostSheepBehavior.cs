using System.Linq;
using UnityEngine;

public class GhostSheepBehavior : AgentBehaviour
{
    private GameObject[] dogs;
    public bool mode = true;
    public AudioSource audio_ghost;
    public AudioSource audio_sheep;


    public void Start(){
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.green, 100);
        dogs = GameObject.FindGameObjectsWithTag("CelluloDogs");
        InvokeRepeating("change_mode", Random.Range(10f, 30f), Random.Range(10f, 30f));
    }

    void OnCollisionEnter(Collision other)
    {
        if (!mode && other.gameObject.CompareTag("CelluloDogs"))
        {
            other.gameObject.GetComponent<Score>().DecreaseSecore(1, 0);
        }
        
    }



    private Steering sheep_mode(){
        const double RADIUS = 4;
        Steering steering = new Steering();
        if (((dogs[0].transform.position - this.transform.position).magnitude <= RADIUS) &&
           ((dogs[1].transform.position - this.transform.position).magnitude > RADIUS))
        {

            Vector3 direction = this.transform.position - dogs[0].transform.position;
            direction /= direction.magnitude;

            steering.linear = direction * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }
        else if (((dogs[1].transform.position - this.transform.position).magnitude <= RADIUS) &&
            ((dogs[0].transform.position - this.transform.position).magnitude > RADIUS))
        {

            Vector3 direction = this.transform.position - dogs[1].transform.position;
            direction /= direction.magnitude;
            steering.linear = direction * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }
        else if (((dogs[0].transform.position - this.transform.position).magnitude <= RADIUS) &&
            ((dogs[1].transform.position - this.transform.position).magnitude <= RADIUS))
        {
            Vector3 direction = (this.transform.position - dogs[0].transform.position) +
                (this.transform.position - dogs[1].transform.position);
            direction /= direction.magnitude;
            steering.linear = direction * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }

        return steering;

    }

    private Steering ghost_mode() {
        Steering steering = new Steering();

        double d0= (dogs[0].transform.position - this.transform.position).magnitude;
        double d1 = (dogs[1].transform.position - this.transform.position).magnitude;

        if (d0 < d1)
        {
            Vector3 direction = dogs[0].transform.position - this.transform.position;
            direction /= direction.magnitude;
            steering.linear = direction * agent.maxAccel ;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));
        }
        else
        {
            Vector3 direction = dogs[1].transform.position - this.transform.position;
            direction /= direction.magnitude;

            steering.linear = direction * agent.maxAccel;
            steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear, agent.maxAccel));

        }

        steering.linear = 0.85f * steering.linear;
        return steering;

    }

    private void change_mode()
    {
        if (Timer.enable)
        {
            mode = !mode;
            if (mode == false)
            {
                agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.yellow, 100);
                audio_ghost.Play();

                dogs[0].GetComponent<CelluloAgent>().MoveOnStone();
                dogs[1].GetComponent<CelluloAgent>().MoveOnStone();

                dogs[0].GetComponent<CelluloAgent>().SetHapticBackdriveAssist(0, 0, 0); //added to disable back drive assist
                dogs[1].GetComponent<CelluloAgent>().SetHapticBackdriveAssist(0, 0, 0);
            }
            else
            {
                audio_sheep.Play();
                agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.green, 100);

                dogs[0].GetComponent<CelluloAgent>().SetHapticBackdriveAssist(0.9f, 0.9f, 0);
                dogs[1].GetComponent<CelluloAgent>().SetHapticBackdriveAssist(0.9f, 0.9f, 0);

                dogs[0].GetComponent<CelluloAgent>().ClearHapticFeedback();
                dogs[1].GetComponent<CelluloAgent>().ClearHapticFeedback(); // added to disable stone effect
            }

        }

    }

    public override Steering GetSteering()
    {

        Steering steering = new Steering();

        //implement your code here.
        if (Timer.enable)
        {
            if (mode)
            {
                steering = sheep_mode();
            }
            else
            {
                steering = ghost_mode();
            }
        }
        return steering;
    }



}
