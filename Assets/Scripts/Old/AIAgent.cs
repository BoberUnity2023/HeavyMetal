using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    CarController controller = null;

    // Waypont system
    int currentWaypointIndex = 0;
    Transform currentWaypointCenter;

    Vector3 currentTargetPosition;
    bool hasValidAreaTarget = false;

    [Header("Area Waypoint Settings")]
    public float areaTargetUpdateDistance = 3f;

    [Header("Debug & Testing")]
    public bool showDebugInfo = true;

    [Header("LiDAR")]
    public bool enableLidar = true;
    public LayerMask detectableLayers = ~0;
    public float maxLidarDistance = 25.0f;
    public int maxLidarRays = 15;

    // Get unstuck system variables
    float stackInterval = 0f;
    float reverseTimer = 0f;
    bool isInReverseMode = false;

    float lastLoopTime = 0.0f;

    void Start()
    {
        controller = gameObject.GetComponent<CarController>();

        if (WaypointManager.Instance != null && WaypointManager.Instance.areaWaypoints.Count > 0)
        {
            var firstArea = WaypointManager.Instance.GetFirstWaypoint();
            if (firstArea != null)
            {
                lastLoopTime = Time.time;
                currentWaypointCenter = firstArea;
                UpdateAreaTargetPosition();
            }
        }
    }

    void UpdateAreaTargetPosition()
    {
        if (WaypointManager.Instance != null && currentWaypointIndex >= 0 &&
            currentWaypointIndex < WaypointManager.Instance.areaWaypoints.Count)
        {
            currentTargetPosition = WaypointManager.Instance.GetRandomPositionInWaypoint(currentWaypointIndex);
            hasValidAreaTarget = true;
        }
    }

    void Update()
    {
        if (hasValidAreaTarget)
        {
            float distanceToTarget = (transform.position - currentTargetPosition).magnitude;
            if (distanceToTarget <= areaTargetUpdateDistance)
            {
                MoveToNextArea();
            }
        }

        if (currentWaypointCenter == null || !hasValidAreaTarget)
        {
            TryFindWaypoint();
            return;
        }

        UpdateStuckDetection();
        DriveToAreaWaypoint();
    }

    void MoveToNextArea()
    {
        if (WaypointManager.Instance == null) return;

        Transform nextArea = WaypointManager.Instance.GetNextWaypoint(currentWaypointIndex);
        if (nextArea != null)
        {
            // Update waypoint index
            currentWaypointIndex++;
            if (currentWaypointIndex >= WaypointManager.Instance.areaWaypoints.Count)
            {
                Debug.Log("Loop time: " + (Time.time - lastLoopTime));
                lastLoopTime = Time.time;
                currentWaypointIndex = 0;
            }

            currentWaypointCenter = nextArea;

            UpdateAreaTargetPosition();
        }
    }

    void TryFindWaypoint()
    {
        if (WaypointManager.Instance != null && WaypointManager.Instance.areaWaypoints.Count > 0)
        {
            var firstArea = WaypointManager.Instance.GetFirstWaypoint();
            if (firstArea != null)
            {
                currentWaypointIndex = 0;
                currentWaypointCenter = firstArea;
                UpdateAreaTargetPosition();
            }
        }
    }

    void DriveToAreaWaypoint()
    {
        if (!hasValidAreaTarget) return;

        float approachFactor = 0.0f;
        if (isInReverseMode)
        {
            controller.SetSteeringAxis(0.0f);
            approachFactor = -0.5f;
            reverseTimer -= Time.deltaTime;
            if (reverseTimer <= 0.0f)
            {
                isInReverseMode = false;
            }
        }
        else
        {
            // Steering
            // Target vector
            Vector3 localTarget = transform.InverseTransformPoint(currentTargetPosition).normalized;
            float steeringAxis = localTarget.x;

            // LiDAR
            if(enableLidar)
            {
                float rayStep = controller.maxSteeringAngle * 2 / maxLidarRays;
                localTarget = (currentTargetPosition - transform.position).normalized;

                Vector3 origin = transform.position + new Vector3(0.0f, 0.8f, 0.0f);

                List<float> results = new();
                for (int i = 0; i < maxLidarRays; i++)
                {
                    float horizontalAngle = -controller.maxSteeringAngle + i * rayStep;
                    Vector3 direction = Quaternion.Euler(0, horizontalAngle, 0) * transform.forward;

                    RaycastHit hit;
                    bool result = Physics.Raycast(origin, direction, out hit, maxLidarDistance, detectableLayers);
                    if (result)
                    {
                        Debug.DrawLine(origin, hit.point, Color.green);
                        results.Add(-1.0f);
                    }
                    else
                    {
                        Debug.DrawLine(origin, origin + direction * maxLidarDistance, Color.red * 0.5f);
                        results.Add(Vector3.Dot(direction, localTarget));
                    }
                }

                float steeringAngle = 0.0f;
                float maxPdf = 0.0f;
                int sideRays = 2;
                for (int i = 0; i < results.Count; i++)
                {
                    float pdfSum = results[i];
                    float div = 1.0f;
                    for (int s = 1; s <= sideRays; s++)
                    {
                        int nIndex = i - s;
                        int pIndex = i + s;
                        if (nIndex >= 0)
                        {
                            pdfSum += results[nIndex];
                            div += 1.0f;
                        }

                        if (pIndex < results.Count)
                        {
                            pdfSum += results[pIndex];
                            div += 1.0f;
                        }
                    }

                    float pdf = pdfSum / div;
                    if (pdf > maxPdf)
                    {
                        maxPdf = pdf;
                        steeringAngle = -controller.maxSteeringAngle + i * rayStep;
                    }
                }

                if(maxPdf > 0.0f) {
                    steeringAxis = steeringAngle / controller.maxSteeringAngle;

                    Vector3 dir = Quaternion.Euler(0, steeringAngle, 0) * transform.forward;
                    Debug.DrawLine(origin, origin + dir * maxLidarDistance, Color.blue);
                }
            }

            controller.SetSteeringAxis(steeringAxis);

            // Speed control
            Vector3 directionToTarget = currentTargetPosition - transform.position;
            directionToTarget.y = 0;
            float distanceFactor = directionToTarget.magnitude * 0.15f;
            float steeringFactor = 1.0f - Mathf.Abs(steeringAxis * 0.5f);
            approachFactor = Mathf.Clamp01(distanceFactor * steeringFactor);
        }

        if (controller.carSpeed < Mathf.Abs(controller.maxSpeed * approachFactor))
        {
            controller.SetThrottle(approachFactor);
        }
        else
        {
            controller.SetThrottle(0.0f);
        }
    }

    void UpdateStuckDetection()
    {
        if (!isInReverseMode)
        {
            if (controller.carRigidbody.linearVelocity.magnitude < 2f)
            {
                stackInterval += Time.deltaTime;
                if (stackInterval > 1.0f)
                {
                    isInReverseMode = true;
                    reverseTimer = 2.0f;
                }
            }
            else
            {
                stackInterval = 0.0f;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebugInfo || controller == null) return;

        // Current target visualization
        if (hasValidAreaTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTargetPosition);
            Gizmos.DrawWireSphere(currentTargetPosition, 0.5f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 3f);

#if UNITY_EDITOR
        string status = $"Area: {currentWaypointIndex}";
        status += $"\nSpeed: {controller.carRigidbody.linearVelocity.magnitude:F1}";
        status += $"\nSteering: {controller.frontLeftCollider.steerAngle:F1}°";

        if (isInReverseMode)
        {
            status += $"\nREVERSING: {reverseTimer:F1}s";
        }

        UnityEditor.Handles.Label(transform.position + Vector3.up * 2, status);
#endif
    }
}
