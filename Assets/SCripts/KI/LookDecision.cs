using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position,
            controller.eyes.forward * controller.enemyStats.lookRange, Color.green);

        //todo: keine raycast, vektor winkel zum spieler, distanz, dann raycast
        if (Physics.SphereCast(controller.eyes.position,
            controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit,
            controller.enemyStats.lookRange))
        {

            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                controller.chaseTarget = hit.transform;
                return true;
            }

        }

        return false;

    }
}