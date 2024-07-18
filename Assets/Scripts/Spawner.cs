using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float currentSpeed;
    [SerializeField]
    private GameObject BeatBoxObj;

    public LaneObject[] laneObjects;
    [Header("Spawner")]
    [SerializeField] private float spawnInterval = 1.0f;

    public GameEvent Event;

    private CountdownTimer spawnTimer;


    private Direction previousDirection = Direction.Right;
    private Lane previousLane = Lane.Lane1;

    private Direction currentDirection = Direction.Right;
    private Lane currentLane = Lane.Lane1;

    private Transform spawnPoint;

 
    private void Awake()
    {
        spawnTimer = new CountdownTimer(spawnInterval);
        spawnTimer.Start();
        spawnTimer.OnTimerStop += SpawnBox;
        spawnTimer.OnTimerStop += () => spawnTimer.Start();

    }
    private void OnEnable()
    {
        Event.OnGameEnd.AddListener(() => spawnTimer.OnTimerStop -= SpawnBox);
    }
    private void OnDisable()
    {
        Event.OnGameEnd.RemoveListener(() => spawnTimer.OnTimerStop -= SpawnBox);
    }

    private void Update()
    {
        spawnTimer.Tick(Time.deltaTime);
    }

    private void SpawnBox()
    {
        currentLane = GetLane();
        previousLane = currentLane;
        currentDirection = GetDirection();
        previousDirection = currentDirection;
        ChooseSpawnPoint(currentLane, currentDirection);
        GameObject spawnedObject = Instantiate(BeatBoxObj, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        currentSpeed = Random.Range(0.5f * speed, speed * 1.5f);
        speed = speed * 1.005f;
        if (rb != null)
        {
            rb.AddForce(currentDirection == Direction.Left ? Vector3.left * currentSpeed : Vector3.right * currentSpeed, ForceMode.Impulse);
        }
    }

    private void ChooseSpawnPoint(Lane lane, Direction direction)
    {
        foreach (LaneObject laneObject in laneObjects)
        {
            if (laneObject.laneID == lane)
            {
                spawnPoint = direction != Direction.Left ? laneObject.LeftSpawnPos : laneObject.RightSpawnPos;
                return;
            }
        }
        Debug.LogError("LaneObject not found for lane: " + lane);
    }

    private Direction GetDirection()
    {
        if (previousDirection == Direction.Left)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.Left;
        }
    }

    private Lane GetLane()
    {
        switch (previousLane)
        {
            case Lane.Lane1:
                return ChooseRandomLane(previousLane);
            case Lane.Lane2:
                return ChooseRandomLane(previousLane);
            case Lane.Lane3:
                return ChooseRandomLane(previousLane);
            default:
                Debug.LogError("Invalid Lane");
                return Lane.Lane1;
        }
    }

    private Lane ChooseRandomLane(Lane current)
    {
        Lane newLane;
        do
        {
            newLane = (Lane)Random.Range(0, 3);
        } while (current == newLane);

        return newLane;
    }


}

public enum Direction
{
    Left,
    Right
}

public enum Lane
{
    Lane1,
    Lane2,
    Lane3
}

