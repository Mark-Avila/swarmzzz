using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySteer : MonoBehaviour
{
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private EnemySteerData enemyData;
    [SerializeField] private float detectionDelay = 0.1f;

    private void Start()
    {
        InvokeRepeating(nameof(PerformDetection), detectionDelay, detectionDelay);
        Debug.Log("Reached start");
    }

    private void PerformDetection()
    {
        Debug.Log("Reached perdetc");
        foreach (Detector detector in detectors)
            detector.Detect(enemyData);
    }
}
