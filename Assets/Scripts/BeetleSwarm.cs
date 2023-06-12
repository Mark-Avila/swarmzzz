using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleSwarm : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject beetle;
     [Tooltip("No. of Aliens"), SerializeField] private int alienNo = 5;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;
    [SerializeField] private AudioClip beetlesAudio;

    private List<BeetleMovement> beetles;

    // Particle Swarm Optimization parameters
    private Vector2[] positions;
    private Vector2[] velocities;
    private Vector2[] pBestPositions;
    private float[] pBestFitness;
    private Vector2 gBestPosition;
    private float gBestFitness;
    private Rigidbody2D targetRb;
    private int swarmSize;

    // Start is called before the first frame update
    void Awake()
    {
        beetles = new List<BeetleMovement>();

        for (int i = 0; i < alienNo; i++)
        {
            GameObject instBeetle= CreateAlien();
            BeetleMovement newBeetle = instBeetle.GetComponentInChildren<BeetleMovement>();
            beetles.Add(newBeetle);
        }

        swarmSize = beetles.Count;

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
            positions[i] = beetles[i].transform.position;
            velocities[i] = Random.insideUnitCircle * maxSpeed;
            pBestPositions[i] = positions[i];
            pBestFitness[i] = Mathf.Infinity;
        }
    }

    void ResetValues()
    {
        beetles.Clear();

        beetles = new List<BeetleMovement>();

        swarmSize = transform.childCount;

        ResetBeetles();

        // Initialize the particle swarm optimization parameters
        positions = new Vector2[swarmSize];
        velocities = new Vector2[swarmSize];
        pBestPositions = new Vector2[swarmSize];
        pBestFitness = new float[swarmSize];
        gBestFitness = Mathf.Infinity;

        // Initialize the positions and velocities of the zombies
        for (int i = 0; i < swarmSize; i++)
        {
            if (beetles[i])
            {
                positions[i] = beetles[i].transform.position;
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

    void Start()
    {
        InvokeRepeating(nameof(ResetValues), 3f, 3f);

        AudioManager.Instance.PlayAudioClip(beetlesAudio, 0.25f);
    }

    private void Update()
    {
        int count = transform.childCount;

        if (count == 0)
        {
            AudioManager.Instance.StopAudioClip(beetlesAudio);
            Destroy(gameObject);
        }
    }

    private void ResetBeetles()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            BeetleMovement currBeetle = child.GetComponentInChildren<BeetleMovement>();

            beetles.Add(currBeetle);
        }

    }

    private GameObject CreateAlien()
    {
        GameObject newBeetle= Instantiate(beetle, transform);

        newBeetle.transform.parent = transform;
        newBeetle.transform.localPosition = Vector2.zero;
        newBeetle.transform.localRotation = Quaternion.identity;

        return newBeetle;
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

            if (beetles[i])
            {
                beetles[i].MoveToPosition(positions[i]);
            }
        }
    }
}
