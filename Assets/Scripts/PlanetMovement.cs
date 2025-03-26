using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public Transform sun;
    public Transform[] otherBodies; // array de planetas
    public float massSun = 1f;
    public float massPlanet;
    public float[] massesOtherBodies; // masas de los otros planetas

    public float initialPos;
    public float initialVelocity;
    public float gravity = 39.478f;

    private Vector3 previosPos;

    void Start()
    {
        transform.position = new Vector3(initialPos, 0, 0);
        initialVelocity = Mathf.Sqrt(gravity * massSun / initialPos);

        Vector3 tangentDirection = new Vector3(0, 0, 1);

        previosPos = transform.position - tangentDirection * initialVelocity * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        // Fuerza Sol
        Vector3 directionToSun = sun.position - transform.position;
        float distanceToSun = directionToSun.magnitude;
        Vector3 acceleration = directionToSun.normalized * (gravity * massSun) / (distanceToSun * distanceToSun);

        // Fuerzas de otros planetas
        for (int i = 0; i < otherBodies.Length; i++)
        {
            if (otherBodies[i] != null && otherBodies[i] != this.transform)
            {
                Vector3 directionToBody = otherBodies[i].position - transform.position;
                float distanceToBody = directionToBody.magnitude;
                float bodyAccelerationMagnitude = (gravity * massesOtherBodies[i]) / (distanceToBody * distanceToBody);
                acceleration += directionToBody.normalized * bodyAccelerationMagnitude;
            }
        }

        //Verlet
        Vector3 newPos = 2 * transform.position - previosPos + acceleration * Mathf.Pow(Time.fixedDeltaTime, 2);
        previosPos = transform.position;
        transform.position = newPos;
    }
}