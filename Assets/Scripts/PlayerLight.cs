using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Animator animator;
    [SerializeField] private Light2D light2D;

    private bool isDead;

    private void Start()
    {
        isDead = animator.GetBool("isDead");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = target.transform.position;

        if (isDead)
        {
            light2D.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        isDead = animator.GetBool("isDead");
    }
}
