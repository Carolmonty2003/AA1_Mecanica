using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public Transform sun;
    public float massSun = 1f;
    public float massPlanet;

    public float initialPos;
    public float initialVelocity;
    public float gravity = 39.478f;

    private Vector3 previosPos;

    void Start()
    {
        transform.position = new Vector3(initialPos, 0, 0);
        initialVelocity = Mathf.Sqrt(gravity / initialPos);

        Vector3 tangentDirection = new Vector3(0, 0, 1);

        previosPos = transform.position - tangentDirection * initialVelocity * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        Vector3 direction = sun.position - transform.position;
        float distance = direction.magnitude;
        float accelerationMagnitude = (gravity * massSun) / (distance * distance);
        Vector3 acceleration = direction.normalized * accelerationMagnitude;

        Vector3 newPos = 2 * transform.position - previosPos + acceleration * Mathf.Pow(Time.fixedDeltaTime, 2);
        previosPos = transform.position;
        transform.position = newPos;
        
    }
}