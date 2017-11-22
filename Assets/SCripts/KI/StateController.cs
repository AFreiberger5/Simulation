using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateController : MonoBehaviour
{

    public State currentState;
    public EnemyStats enemyStats;
    public Transform eyes;
    public State remainState;


    public NavMeshAgent navMeshAgent;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;

    private bool aiActive;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        nextWayPoint = 1;
        
    }

    public void SetupAI(bool aiActivationFromTriggerZone, List<Transform> wayPointsFromTriggerZone)
    {
        
        wayPointList = wayPointsFromTriggerZone;
        aiActive = aiActivationFromTriggerZone;
        navMeshAgent.enabled = aiActive;
       
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

   void OnDrawGizmos()
   {
       if (currentState != null && eyes != null)
       {
           Gizmos.color = currentState.sceneGizmoColor;
           Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
           
       }
   }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}