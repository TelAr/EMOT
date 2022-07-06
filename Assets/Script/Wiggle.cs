using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//class for wiggle effect on position
public class Wiggle : MonoBehaviour
{
    public float speed = 1;
    public float amplitude = 2;
    public int octaves = 4;

    Vector3 destination;
    Vector3 vel = Vector3.zero;
    int currentTime = 0;

    void FixedUpdate()
    {
        // if number of frames played since last change of direction > octaves create a new destination
        if (currentTime > octaves)
        {
            currentTime = 0;
            destination = generateRandomVector(amplitude);
        }

        // smoothly moves the object to the random destination
        transform.position = Vector3.SmoothDamp(transform.position, destination,ref vel, speed);

        currentTime++;
    }

    // generates a random vector based on a single amplitude for x y and z
    Vector3 generateRandomVector(float amp)
    {
        Vector3 result = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            float x = Random.Range(-amp, amp);
            result[i] = x;
        }
        return result;
    }
}
