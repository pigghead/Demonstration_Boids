using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    //public GameObject target;  // something to aim at when wandering
    public CharacterController charController;  // our agent's character controller
    private GameManager GM;  // game manager

    public float mass = 10.0f;
    public float wanderRadius = 1.0f;
    public float wanderRate = 0.25f;

    // START **WEIGHTS**
    [Range(0, 1)]
    public float wanderWeight;

    [Range(0, 1)]
    public float cohesionWeight;

    [Range(1, 3)]
    public float avoidWeight;

    public float fleeWeight = 0.25f;
    public float chaseWeight = 0.25f;
    // END **WEIGHTS**

    [Range(4, 12)]
    public float maxSpeed = 4.0f;

    public float maxTurnSpeed = 0.25f;
    public float radius = 0.5f;

    public float avoidRate = 0.05f;

    //public GameObject targetIndicator;  // represent the target we are aiming at

    private float wanderRotation = 0f;  // current rotation of wander target
    protected Vector3 Velocity;
    private Vector3 Acceleration;

    private Vector3 forwardVector;
    private Vector3 rightVector;

    private int numOfBoids = 12;

    //private BoidController[] others;

    protected void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Setting weights
        wanderWeight = GM.GM_wanderWeight;
        cohesionWeight = GM.GM_cohesionWeight;
        avoidWeight = 1.5f;

        // Start off with a random rotation
        wanderRotation = Random.Range(0f, Mathf.PI * 2f);

        charController = GetComponent<CharacterController>();

        Velocity = Vector3.zero;
        Acceleration = Vector3.zero;

        //others = new BoidController[numOfBoids];
    }

    //private void CalcSteering()
    //{
    //    Vector3 fleePosition = fleeTarget.transform.position;
    //    ApplyForce(Wander() * wanderWeight);
    //    ApplyForce(Flee(fleePosition) * fleeWeight);
    //}

    //protected abstract void CalcSteering();

    public void TestCalcSteering()
    {
        Vector3 futurePos = FuturePosition(2.5f);
        float xWeight = Mathf.Abs(futurePos.x) > 39 ? 1f + Mathf.Abs(futurePos.x) : 0f;
        float zWeight = Mathf.Abs(futurePos.z) > 39 ? 1f + Mathf.Abs(futurePos.z) : 0f;

        Vector3 wallForce = AvoidWall();
        ApplyForce((xWeight + zWeight) * wallForce);

        ApplyForce(Wander() * wanderWeight);
        ApplyForce(Cohesion() * cohesionWeight);
        ApplyForce(Avoidance());
        //ApplyForce(new Vector3(0, 0, 0) * 1.25f);

        //ApplyForce(Wander() * wanderRate);
        //ApplyForce(Cohesion());
    }

    protected void ApplyForce(Vector3 force)
    {
        force.y = 0;
        Acceleration += force / mass;
    }

    /// <summary>
    /// Looks x amount of seconds into the future for a position
    /// </summary>
    /// <param name="time">Amount of time</param>
    /// <returns></returns>
    public Vector3 FuturePosition(float time)
    {
        return this.transform.position + (Velocity * time);
    }

    /// <summary>
    /// Seek the provided target and chase it.
    /// </summary>
    /// <param name="targetPos">Position of the Target</param>
    /// <returns></returns>
    protected Vector3 Seek(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - this.transform.position;  // Distance between us and the target position
        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;  // The vector between us is then normalized (valued at 1) then multiplied by our maxSpeed
        Vector3 steeringForce = desiredVelocity - Velocity;

        //return Vector3.ClampMagnitude(steeringForce, maxTurnSpeed);
        return Clamp(steeringForce, maxTurnSpeed);
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        Vector3 toTarget = this.transform.position - targetPos;
        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - Velocity;

        return Clamp(steeringForce, maxTurnSpeed);
    }
    
    public Vector3 Arrive(Vector3 targetPos, float threshold, float radii)
    {
        Vector3 toTarget = targetPos = this.transform.position;  // Dist between us and target position
        Vector3 desiredVelocity;

        // If the distance between us and the target minus the radius of our line of sight is less than the threshold,
        // we have arrived at the target (within an acceptable amount)
        if (toTarget.magnitude - radii < threshold)
        {
            // Find the percentage of distance left between us and the target
            float percentFromCenter = (toTarget.magnitude - radii) / threshold;
            float fractionOfMaxSpeed = percentFromCenter * maxSpeed;

            // Decrease our speed as we get closer
            desiredVelocity = toTarget.normalized * fractionOfMaxSpeed;
        } else
        // Otherwisde, move at the maxSpeed determined
        {
            desiredVelocity = toTarget.normalized * maxSpeed;
        }

        // Set steering forces
        Vector3 steeringForce = desiredVelocity - Velocity;

        // Clamp calculated steering force against our maxTurnSpeed
        //return Vector3.ClampMagnitude(steeringForce, maxTurnSpeed);
        return Clamp(steeringForce, maxTurnSpeed);
    }

    public Vector3 AvoidWall()
    {
        return Seek(Vector3.zero);
    }

    public Vector3 Wander()
    {
        // change the wanderRotation value by a random amount
        // keep it as natural looking as possible
        wanderRotation += Random.Range(-wanderRate, wanderRate);

        // start from futurepos
        Vector3 Future = FuturePosition(3f);

        // construct unit rotation vector from wanderRotation
        Vector3 unitRotation = ConstructUnitVector(wanderRotation);

        // the wander position is our future position + vector of magnitude wanderRadius
        // pointing in the direction of our unit rotation
        Vector3 FinalWanderPosition = Future + (unitRotation * wanderRadius);

        // ensure the Y's are equal
        FinalWanderPosition.y = transform.position.y;

        // seek result
        return Seek(FinalWanderPosition);
    }

    /// <summary>
    /// How well will the Boids flock with one another
    /// </summary>
    /// <returns>A Vector3 that will correct a Boid's path</returns>
    public Vector3 Cohesion()
    {
        Vector3 avgPosition = Vector3.zero;  // Perceived center of the flock

        // Find the average position of all the boids (each position summed up divided by the number of boids)
        foreach (var boid in GM.Boids)
        {
            // Do not include this boid
            if(boid != this)
            {
                avgPosition += boid.transform.position;
            }
        }

        avgPosition = avgPosition / (GM.Boids.Length - 1);
        // others.Length - 1 to not account for this boid

        // 1% towards the center
        return (avgPosition - this.transform.position) / 100;
    }

    /// <summary>
    /// How well 
    /// </summary>
    /// <param name="b">Boid performing the calculation</param>
    public Vector3 Avoidance()
    {
        Vector3 displacement = Vector3.zero;

        foreach (var boid in GM.Boids)
        {
            // Do not include this boid
            if (boid != this)
            {
                // Number after '<' represents the distance between any given boid and this one
                if (Mathf.Abs(boid.transform.position.sqrMagnitude - this.transform.position.sqrMagnitude) < 0.7)
                {
                    displacement -= (boid.transform.position - this.transform.position);
                }
            }
           // Debug.DrawLine(this.transform.position, boid.transform.position, Color.cyan);
        }

        foreach (var obstacle in GM.Obstacles)
        {
            // Number after '<' represents the distance between any given boid and an obstacle
            if (Mathf.Abs(obstacle.transform.position.sqrMagnitude - this.transform.position.sqrMagnitude) < 0.2)
            {
                displacement -= (obstacle.transform.position - this.transform.position);
            }
            // Debug.DrawLine(this.transform.position, obstacle.transform.position, Color.red);
        }
        return displacement;
    }

    void LateUpdate()
    {
        //CalcSteering();

        TestCalcSteering();

        Velocity += Acceleration;
        //Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
        Velocity = Clamp(Velocity, maxSpeed);
        this.charController.Move(Velocity * Time.deltaTime);

        //this.targetIndicator.transform.position = FuturePosition(3f);

        Acceleration = Vector3.zero;

        this.transform.rotation = Quaternion.identity;

        //Debug.DrawLine(this.transform.position, this.targetIndicator.transform.position, Color.red);
        //Debug.DrawLine(this.transform.position, this.transform.up.normalized, Color.green);
        Debug.DrawLine(this.transform.position, FuturePosition(2.5f), Color.red);
        Debug.Log(FuturePosition(2.5f));
    }

    // ********************
    // * HELPER FUNCTIONS *
    // ********************
    // Used to calculate all necessary vectors (forward, right, up)
    private void VectorCalculator()
    {
        forwardVector = this.transform.position - transform.forward;
        rightVector = Vector3.Cross(forwardVector, transform.up);
    }

    private Vector3 ConstructUnitVector(float wR)
    {
        return new Vector3(Mathf.Cos(wR), 0, Mathf.Sin(wR));
    }

    private float Distance(BoidController b1, BoidController b2)
    {
        Vector3 b1Transform = b1.transform.position;
        Vector3 b2Transform = b2.transform.position;

        return Mathf.Sqrt(
            (b1Transform.x - b2Transform.x) *
            (b1Transform.y - b2Transform.y) *
            (b1Transform.z - b2Transform.z)
            );
        // Calculate the distance between boids
        //return 0f;
    }

    private Vector3 Clamp(Vector3 v, float magnitudeLimit)
    {
        float mag = v.magnitude;
        if (mag > magnitudeLimit)
        {
            v *= magnitudeLimit / v.magnitude;
        }
        return v;
    }
}
