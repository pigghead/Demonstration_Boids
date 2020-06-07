using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : BoidController
{
    //public GameObject target;  // what is being sought
    public float threshold = 2f;
    public float wanderWeight = 0.125f;
    //public float chaseWeight = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Velocity = new Vector3(Random.Range(0, maxSpeed) - maxSpeed / 2f, 0, Random.Range(0, maxSpeed) - maxSpeed / 2f);
    }

    //protected override void CalcSteering()
    //{
    //    Vector3 futurePos = FuturePosition(1f);
    //    float xWeight = Mathf.Abs(futurePos.x) > 20 ? 1f + Mathf.Abs(futurePos.x) - 20f : 0f;
    //    float zWeight = Mathf.Abs(futurePos.z) > 20 ? 1f + Mathf.Abs(futurePos.z) - 20f : 0f;

    //    Vector3 wallForce = AvoidWall();
    //    ApplyForce((xWeight + zWeight) * wallForce);

    //    ApplyForce(Wander() * wanderWeight);

    //    //ApplyForce()
    //}
}
