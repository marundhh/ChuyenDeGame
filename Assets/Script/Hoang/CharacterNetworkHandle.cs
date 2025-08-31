using Fusion;
using UnityEngine;
using Invector.vCharacterController;


public class CharacterNetworkHandler : NetworkBehaviour
{
    private vThirdPersonController controller;
    private vThirdPersonInput input;


    public override void Spawned()
    {
        controller = GetComponent<vThirdPersonController>();
        input = GetComponent<vThirdPersonInput>();

        if (!Object.HasInputAuthority)
        {
            // Tắt input với player không phải local
            input.enabled = false;
        }

    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var jumpInput = Input.GetButtonDown("Jump"); // chỉ nhận 1 lần khi nhấn

            RPC_SetInput(moveInput, jumpInput);
        }
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetInput(Vector2 moveInput, bool jumpInput)
    {
        if (controller != null)
        {
            controller.input.x = moveInput.x;
            controller.input.y = moveInput.y;
            if (jumpInput) controller.Jump();
        }
    }
}