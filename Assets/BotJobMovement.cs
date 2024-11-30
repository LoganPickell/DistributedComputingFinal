using Pathfinding;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;

public class BotJobMovement : MonoBehaviour
{
    public Seeker seeker;
    public Transform target;
    public float speed = 5f;

    private NativeArray<Vector3> waypoints; // Waypoints from A*
    private int currentWaypointIndex = 0;

    private bool pathReady = false;

    void Start()
    {
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            // Store waypoints in a NativeArray
            waypoints = new NativeArray<Vector3>(p.vectorPath.Count, Allocator.Persistent);
            for (int i = 0; i < p.vectorPath.Count; i++)
            {
                waypoints[i] = p.vectorPath[i];
            }
            currentWaypointIndex = 0;
            pathReady = true;
        }
    }

    void Update()
    {
        if (pathReady)
        {
            // Schedule the movement job
            NativeArray<Vector3> botPositions = new NativeArray<Vector3>(1, Allocator.TempJob);
            botPositions[0] = transform.position;

            var moveJob = new MoveAlongPathJob
            {
                botPositions = botPositions,
                waypoints = waypoints,
                currentWaypointIndex = currentWaypointIndex,
                speed = speed,
                deltaTime = Time.deltaTime
            };

            JobHandle jobHandle = moveJob.Schedule();
            jobHandle.Complete();

            // Update the bot's position
            transform.position = botPositions[0];

            // Cleanup
            botPositions.Dispose();
        }
    }

    private void OnDestroy()
    {
        if (waypoints.IsCreated)
            waypoints.Dispose();
    }

    [BurstCompile]
    public struct MoveAlongPathJob : IJob
    {
        public NativeArray<Vector3> botPositions;
        [ReadOnly] public NativeArray<Vector3> waypoints;
        public int currentWaypointIndex;
        public float speed;
        public float deltaTime;

        public void Execute()
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                Vector3 currentPosition = botPositions[0];
                Vector3 targetPosition = waypoints[currentWaypointIndex];

                Vector3 direction = (targetPosition - currentPosition).normalized;
                Vector3 newPosition = currentPosition + direction * speed * deltaTime;

                if (Vector3.Distance(newPosition, targetPosition) < 0.1f)
                {
                    // Move to the next waypoint
                    currentWaypointIndex++;
                }

                botPositions[0] = newPosition;
            }
        }
    }
}
