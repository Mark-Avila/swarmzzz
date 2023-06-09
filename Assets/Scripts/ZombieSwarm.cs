using System.Collections.Generic;
using UnityEngine;

public class ZombieSwarm : MonoBehaviour
{
    [SerializeField] private GameObject target; // The player GameObject
    [SerializeField] private GameObject zombie;
    [SerializeField] private int maxZombies = 5;
    [SerializeField] private float maxSpeed; // Maximum speed of the zombies
    [SerializeField] private float maxForce; // Maximum force that can be applied to the zombies
    [SerializeField] private AudioClip zombieAudio; // Maximum force that can be applied to the zombies

    private List<ZombieMovement> zombies; // List of all zombies in the swarm

    // Particle Swarm Optimization parameters
    private Vector2[] positions;
    private Vector2[] velocities;
    private Vector2[] pBestPositions;
    private float[] pBestFitness;
    private Vector2 gBestPosition;
    private float gBestFitness;
    private Rigidbody2D targetRb;
    private int swarmSize;

    void Awake()
    {
        // Initialize the zombies list
        zombies = new List<ZombieMovement>();

        for (int i = 0; i < maxZombies; i++)
        {
            GameObject instZombie = CreateZombie();
            ZombieMovement newZombie = instZombie.GetComponentInChildren<ZombieMovement>();
            zombies.Add(newZombie);
        }

        swarmSize = zombies.Count;

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

    void Start()
    {
        InvokeRepeating(nameof(ResetValues), 0.5f, 0.5f);

        AudioManager.Instance.PlayAudioLoop2d(zombieAudio);
    }

    void ResetValues()
    {
        zombies.Clear();

        zombies = new List<ZombieMovement>();

        swarmSize = transform.childCount;

        ResetZombies();

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the zombies
        for (int i = 0; i < swarmSize; i++)
        {
            if (zombies[i])
            {
                positions[i] = zombies[i].transform.position;
            } 
            else
            {
                positions[i] = Vector2.zero;
            }
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }
    }

    private void ResetZombies()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            ZombieMovement currZombie = child.GetComponentInChildren<ZombieMovement>();

            zombies.Add(currZombie);
        }
    }

    private GameObject CreateZombie()
    {
        GameObject newZombie = Instantiate(zombie, transform);

        newZombie.transform.parent = transform;

        newZombie.transform.localPosition = Vector2.zero;
        newZombie.transform.localRotation = Quaternion.identity;

        return newZombie;
    }

    void FixedUpdate()
    {
        // Update the positions and velocities of the zombies using particle swarm optimization
        for (int i = 0; i < swarmSize; i++)
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

            if (zombies[i])
            {
                zombies[i].MoveTowards(positions[i], maxSpeed);
            }
        }
    }
}