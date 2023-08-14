using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager character;

        private int horizontal;

        private int vertical;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            // Can be used the character from attempting new actions
            // For example, if you get damaged, and begin performing a damage animation
            // This flag will turn true if you are stunned
            // We can then check for this before attempting new actions
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            // character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}
