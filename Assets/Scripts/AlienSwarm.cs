using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSwarm : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject alien;
    [Tooltip("No. of Aliens")] [SerializeField] private int alienNo = 5;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;
    [SerializeField] private AudioClip alienAudio;

    private List<AlienMovement> aliens;

    // Particle Swarm Optimization parameters
    private Vector2[] positions;
    private Vector2[] velocities;
    private Vector2[] pBestPositions;
    private float[] pBestFitness;
    private Vector2 gBestPosition;
    private float gBestFitness;
    private Rigidbody2D targetRb;
    private int swarmSize;

    void Start()
    {
        InvokeRepeating(nameof(ResetValues), 5f, 5f);

        AudioManager.Instance.PlayAudioClip(alienAudio);
    }

    // Start is called before the first frame update
    void Awake()
    {
        aliens = new List<AlienMovement>();

        for (int i = 0; i < alienNo; i++)
        {
            GameObject instAlien = CreateAlien();
            AlienMovement newAlien = instAlien.GetComponentInChildren<AlienMovement>();
            aliens.Add(newAlien);
        }

        swarmSize = aliens.Count;

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
            positions[i] = aliens[i].transform.position;
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }
    }

    void ResetValues()
    {
        aliens.Clear();

        aliens = new List<AlienMovement>();

        swarmSize = transform.childCount;

        ResetAliens();

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the zombies
        for (int i = 0; i < swarmSize; i++)
        {
            if (aliens[i])
            {
                positions[i] = aliens[i].transform.position;
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

    private void Update()
    {
        int count = transform.childCount;

        if (count == 0)
        {
            AudioManager.Instance.StopAudioClip(alienAudio);
            Destroy(gameObject);
        }
    }

    private void ResetAliens()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            AlienMovement currAlien = child.GetComponentInChildren<AlienMovement>();

            aliens.Add(currAlien);
        }
    }

    private GameObject CreateAlien()
    {
        GameObject newAlien = Instantiate(alien, transform);

        newAlien.transform.parent = transform;
        newAlien.transform.localPosition = Vector2.zero;
        newAlien.transform.localRotation = Quaternion.identity;

        return newAlien;
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

            if (aliens[i])
            {
                aliens[i].MoveTowards(positions[i], maxSpeed);
            }
        }
    }
}
