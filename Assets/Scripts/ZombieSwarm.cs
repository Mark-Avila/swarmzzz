using System.Collections.Generic;
using UnityEngine;

public class ZombieSwarm : MonoBehaviour
{
    public GameObject target; // The player GameObject
    
    //public int swarmSize; // Number of zombies in the swarm
    public float maxSpeed; // Maximum speed of the zombies
    public float maxForce; // Maximum force that can be applied to the zombies

    private List<ZombieMovement> zombies; // List of all zombies in the swarm

    // Particle Swarm Optimization parameters
    private Vector2[] positions;
    private Vector2[] velocities;
    private Vector2[] pBestPositions;
    private float[] pBestFitness;
    private Vector2 gBestPosition;
    private float gBestFitness;
    private Rigidbody2D targetRb;

    void Start()
    {
        InvokeRepeating(nameof(ResetValues), 0.5f, 0.5f);

        // Initialize the zombies list
        zombies = new List<ZombieMovement>();

        // Find all zombies in the scene and add them to the list
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Tiny Zombie");

        foreach (GameObject zombie in zombieObjects)
        {
            ZombieMovement newZombie = zombie.GetComponent<ZombieMovement>();
            zombies.Add(newZombie);
        }

        int swarmSize = zombies.Count;

        targetRb = target.GetComponent<Rigidbody2D>();

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the zombies
        for (int i = 0; i < swarmSize; i++)
        {
            positions[i] = zombies[i].transform.position;
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }
    }

    void ResetValues()
    {
        int swarmSize = zombies.Count;

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the zombies
        for (int i = 0; i < swarmSize; i++)
        {
            positions[i] = zombies[i].transform.position;
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }
    }

    void FixedUpdate()
    {
        // Update the positions and velocities of the zombies using particle swarm optimization
        for (int i = 0; i < zombies.Count; i++)
        {
            Vector2 predictedPosition = (Vector2)target.transform.position + targetRb.velocity;

            // Calculate the fitness of the current zombie
            float fitness = Vector2.Distance(positions[i], predictedPosition);

            // Update the personal best position and fitness of the current zombie
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

            // Calculate the new velocity of the current zombie
            Vector2 acceleration = (pBestPositions[i] - positions[i]) + (gBestPosition - positions[i]);
            acceleration = Vector2.ClampMagnitude(acceleration, maxForce);
            velocities[i] = Vector2.ClampMagnitude(velocities[i] + acceleration, maxSpeed);

            // Update the position of the current zombie
            positions[i] += velocities[i] * Time.fixedDeltaTime;

            //TODO: Modify for RigidBody
            zombies[i].MoveTowards(positions[i], maxSpeed);
        }
    }
}