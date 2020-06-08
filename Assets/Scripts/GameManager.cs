using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Boid;
    public GameObject[] Boids;

    public GameObject Obstacle;
    public List<GameObject> Obstacles;

    [SerializeField, Range(0,15)]
    public float GM_wanderWeight;

    [SerializeField, Range(0, 1)]
    public float GM_cohesionWeight;

    // Start is called before the first frame update
    void Start()
    {
        Boids = new GameObject[12];
        Obstacles = new List<GameObject>();

        GM_wanderWeight = 0.75f;
        GM_cohesionWeight = 0.12f;

        // Instantiate Boids
        for (int i = 0; i < Boids.Length; i++)
        {
            float rX = Random.Range(-40f, 40f);
            float rZ = Random.Range(-40f, 40f);

            Boids[i] = Instantiate(Boid, new Vector3(rX, 0, rZ), Quaternion.identity);
        }

        // Instantiate Obstacles
        int numObstacles = Random.Range(3, 12);
        for (int i = 0; i < numObstacles; i++)
        {
            float rX = Random.Range(-40f, 40f);
            float rZ = Random.Range(-40f, 40f);

            Obstacles.Add(Instantiate(Obstacle, new Vector3(rX, 0, rZ), Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
