using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMove : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        RaycastHit m_HitInfo = new RaycastHit();
        private Animator m_Animator;
        private bool isMoving = false;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            // If the left mouse button is clicked
            if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                // Create a ray from the camera to the mouse position
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                {
                    // Move the agent to the point where the ray hits
                    m_Agent.SetDestination(m_HitInfo.point);
                    isMoving = true;
                }
            }

            // If the agent has reached its destination or isn't moving
            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance && m_Agent.velocity.sqrMagnitude == 0)
            {
                isMoving = false;
            }

            // Set the animation based on the moving state
            m_Animator.SetBool("Running", isMoving);
        }

        void OnAnimatorMove()
        {
            // Update speed based on animator's movement
            if (m_Animator.GetBool("Running"))
            {
                m_Agent.speed = m_Animator.deltaPosition.magnitude / Time.deltaTime;
            }
        }
    }
}
