using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class AI_Scripts
{
    //UTILITY FUNCTIONS
    public static class Utility_Functions
    {
        public static Vector3 RandomPointInNavSphere(Vector3 origin, float radius, int layerMaskask)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, layerMaskask))
            {
                return hit.position;
            }
            return origin;
        }
    }

    // MOVEMENT SCRIPTS
    public static class Mov_Scripts
    {
        public static void SeekTarget(NavMeshAgent agent, Transform target)
        {
            agent.SetDestination(target.transform.position);
        }

        public static void Seek(NavMeshAgent agent, Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public static void Flee(NavMeshAgent agent, Transform target, Vector3 location)
        {
            Vector3 fleeVector = location - target.transform.position;
            agent.SetDestination(target.transform.position - fleeVector);
        }

        //Needs to be called as a Corutine
        public static IEnumerator Wander(NavMeshAgent agent, float wanderRadius, int layermask, float delay = 0)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Seek(agent, Utility_Functions.RandomPointInNavSphere(agent.transform.position, wanderRadius, layermask));
            }

            yield return new WaitForSeconds(delay);
        }

        //Dir = if it goes from first to last or reverse (0 || 1)
        public static int FollowPath(NavMeshAgent agent, int dir, int wpIndex, GameObject[] waypoints)
        {
            if (dir == 0)
            {
                wpIndex = (wpIndex + 1) % waypoints.Length;
            }
            else
            {
                wpIndex = (wpIndex - 1) % waypoints.Length;
                if (wpIndex <= 0)
                {
                    wpIndex = waypoints.Length;
                }
            }

            agent.destination = waypoints[wpIndex].transform.position;

            return wpIndex;
        }
    }

    //DETECTION SCRIPTS
    public static class Detection_Scripts
    {
        public static bool FindInCameraFructum(string tag, GameObject target, Camera frustum, LayerMask mask, out RaycastHit hit)
        {
            hit = new RaycastHit();

            Renderer targetRenderer = target.GetComponent<Renderer>();

            if (!frustum.CompareTag("MainCamera"))
            {
                frustum.cullingMask = mask;
            }

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);
            if (GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds))
            {
                // The object's bounding box is within the camera's frustum.
                // Now, perform a raycast to check if it's visible.
                Ray ray = frustum.ViewportPointToRay(frustum.WorldToViewportPoint(target.transform.position));

                if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
                {
                    if (hit.collider.CompareTag(tag))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
