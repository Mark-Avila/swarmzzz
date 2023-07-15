using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarmBase : MonoBehaviour
{
    //[SerializeField] private GameObject target; // The player GameObject
    [Tooltip("Enemy prefab"), SerializeField] private GameObject enemy;
    [Tooltip("Number of enemies"), SerializeField] private int number;
    [Tooltip("Max Enemy speed"), SerializeField] private float maxSpeed; // Maximum speed of the enemys
    [Tooltip("Max Enemy movement force"), SerializeField] private float maxForce; // Maximum force that can be applied to the enemys
    [Tooltip("Use MovePosition"), SerializeField] private bool useMovePosition = false;
    [SerializeField] private float enemyVolume;

    private List<EnemyMovement> enemys; // List of all enemies in the swarm

    // Particle Swarm Optimization parameters
    private Vector2[] positions;
    private Vector2[] velocities;
    private Vector2[] pBestPositions;
    private float[] pBestFitness;
    private Vector2 gBestPosition;
    private float gBestFitness;
    private Rigidbody2D targetRb;
    private int swarmSize;
    private GameObject target;

    void Start()
    {
        // Initialize the enemys list
        enemys = new List<EnemyMovement>();

        for (int i = 0; i < number; i++)
        {
            GameObject instEnemy = CreateEnemy();
            EnemyMovement newEnemy = instEnemy.GetComponent<EnemyMovement>();
            enemys.Add(newEnemy);
        }

        target = GameObject.FindWithTag("Player");
        swarmSize = enemys.Count;
        targetRb = target.GetComponent<Rigidbody2D>();

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the enemys
        for (int i = 0; i < swarmSize; i++)
        {
            positions[i] = enemys[i].transform.position;
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }

    }

    //For Debugging purpose
    void FixedUpdate()
    {
        // Update the positions and velocities of the enemys using particle swarm optimization
        for (int i = 0; i < swarmSize; i++)
        {
            Debug.Log(swarmSize);

            Vector2 predictedPosition = (Vector2)target.transform.position + targetRb.velocity;

            // Calculate the fitness of the current enemy
            float fitness = Vector2.Distance(positions[i], predictedPosition);

            // Update the personal best position and fitness of the current enemy
            if (fitness < pBestFitness[i])
            {
                pBestPositions[i] = positions[i];
                pBestFitness[i] = fitness;
            }

            // Update the global best position and fitness if necessary
            if (fitness < gBestFitness)
            {
                gBestPosition = positions[i];
                gBestFitness = fitness;
            }

            // Calculate the new velocity of the current enemy
            Vector2 acceleration = (pBestPositions[i] - positions[i]) + (gBestPosition - positions[i]);
            acceleration = Vector2.ClampMagnitude(acceleration, maxForce);
            velocities[i] = Vector2.ClampMagnitude(velocities[i] + acceleration, maxSpeed);

            // Update the position of the current enemy
            positions[i] += velocities[i] * Time.fixedDeltaTime;

            // If enemy instantiate 
            Vector2 newPosition = positions[i];

            if (useMovePosition)
                enemys[i].MoveToPosition(newPosition);
            else
                enemys[i].MoveTowards(newPosition, maxSpeed);
        }
    }

    private GameObject CreateEnemy()
    {
        GameObject newEnemy = Instantiate(enemy, transform);

        newEnemy.transform.localPosition = Vector2.zero;
        newEnemy.transform.localRotation = Quaternion.identity;

        return newEnemy;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetNumberOfEnemies(int number)
    {
        this.number = number;
    }
}
