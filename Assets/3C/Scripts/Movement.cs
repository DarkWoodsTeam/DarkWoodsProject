using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Player player;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 1.5f;
    public void OnMovePlayer(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            player.StartPlayerMove(walkSpeed);

        }
    }
    public void OnTurnCameraRight(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Vector2 vector2 = ctx.ReadValue<Vector2>();
            if(vector2.x < 0)
            {
                player.StartPlayerTurn(turnSpeed, false , false);
            }
            else if(vector2.x > 0)
            {
                player.StartPlayerTurn(turnSpeed, true, false);
            }
            else if (vector2.y < 0)
            {
                player.StartPlayerTurn(turnSpeed * 2, true, true);
            }           
        }      
    }
}
