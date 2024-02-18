using System;
using UnityEngine;
using UnityEngine.AI;

public class SCP173_Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,Chase,Attack
    }
    [SerializeField] private EnemyState state;
    private Transform targetTransform;
    private NavMeshAgent navMeshAgent;

    [SerializeField] Renderer renderer;

    private Animator animator;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();    
        targetTransform = FindObjectOfType<FirstPersonController>().transform;
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        switch(state)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:

                break;
        }
    }

    private void HandleChase()
    {
        if(targetTransform != null)
        {
            if(!IsInLineOfSight() || IsBehindWalls())
            {
                navMeshAgent.SetDestination(targetTransform.position);
                animator.speed = 1;
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
                animator.speed = 0;
            }

        }
        else
        {
            Debug.LogError("No Player Reference");
        }
    }

    private bool IsBehindWalls()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = (targetTransform.position - transform.position).normalized;
        RaycastHit hit;
        float distance = Vector3.Distance(targetTransform.position, transform.position);

        if(Physics.Raycast(ray,out hit,distance))
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.TryGetComponent(out FirstPersonController player))
                {
                    //Enemy Has Clear view
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsInLineOfSight()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if(GeometryUtility.TestPlanesAABB(planes,renderer.bounds))
        {
            return true;
        }
        return false;
    }
}
 