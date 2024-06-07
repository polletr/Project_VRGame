using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField]
    private float maxHeight = 10f; // Maximum height for objects
    [SerializeField]
    private float minHeight = 5f; // Maximum height for objects

    [SerializeField]
    private float spawnInterval = 1f; // Time interval between spawns

    private float timeSinceLastSpawn;

    [SerializeField]
    private float horizontalDistance;
    private float gravity = 9.81f;
    private float initialSpeedX;


    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawn = spawnInterval; // Ensure immediate first spawn
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObject();
            timeSinceLastSpawn = 0f;
        }
    }


    private void SpawnObject()
    {
        if (gameObjects.Count == 0) return;

        // Choose a random object to spawn
        GameObject objectToSpawn = gameObjects[Random.Range(0, gameObjects.Count)];

        // Set the spawn position (constant)
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Instantiate the object at the spawn position
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Apply an initial upward force to the spawned object
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float height = Random.Range(minHeight, this.maxHeight);

            // Calculate the initial vertical speed to reach the maxHeight
            float initialSpeedY = Mathf.Sqrt(2 * gravity * height);

            // Calculate the time to reach the maxHeight and back to the spawn height
            float timeToApex = initialSpeedY / gravity;
            float totalTime = 2 * timeToApex;

            // Calculate the required initial horizontal speed
            initialSpeedX = horizontalDistance / totalTime;

            Vector3 initialVelocity = new Vector3(initialSpeedX, initialSpeedY, 0);

            rb.velocity = initialVelocity;
        }
    }

}
