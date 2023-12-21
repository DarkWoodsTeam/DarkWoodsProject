using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

public class DW_CanAttack : Node
{
    private RaycastHit hit;
    private GameObject Enemy;
    private GameObject Player;
    private float attack_range;
    private Animator anim;

    public DW_CanAttack(GameObject enemy, GameObject player, float attackrange, Animator animator)
    {
        Enemy = enemy;
        Player = player;
        attack_range = attackrange;
        anim = animator;
    }

    public override NodeState Evaluate()
    {
        Vector3 distance = Player.transform.position - Enemy.transform.position;
        if (Physics.Raycast(Enemy.transform.position, distance / distance.magnitude, out hit, attack_range))
        {
            Debug.DrawRay(Enemy.transform.position, distance / distance.magnitude * attack_range, Color.blue);
        }
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsAttacking", true);
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
