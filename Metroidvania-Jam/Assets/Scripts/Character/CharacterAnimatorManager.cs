using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager character;
        private int horizontal;
        private int vertical;

        [Header("Damage Animations")]
        private string lastDamageAnimationPlayed;

        [SerializeField] private string hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
        [SerializeField] private string hit_Forward_Medium_02 = "Hit_Forward_Medium_02";

        [SerializeField] private string hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
        [SerializeField] private string hit_Backward_Medium_02 = "Hit_Backward_Medium_02";

        [SerializeField] private string hit_Left_Medium_01 = "Hit_Left_Medium_01";
        [SerializeField] private string hit_Left_Medium_02 = "Hit_Left_Medium_02";

        [SerializeField] private string hit_Right_Medium_01 = "Hit_Right_Medium_01";
        [SerializeField] private string hit_Right_Medium_02 = "Hit_Right_Medium_02";

        [SerializeField] private List<string> forward_Medium_Damages = new();
        [SerializeField] private List<string> backward_Medium_Damages = new();
        [SerializeField] private List<string> left_Medium_Damages = new();
        [SerializeField] private List<string> right_Medium_Damages = new();

        public List<string> Forward_Medium_Damages => forward_Medium_Damages;
        public List<string> Backward_Medium_Damages => backward_Medium_Damages;
        public List<string> Left_Medium_Damages => left_Medium_Damages;
        public List<string> Right_Medium_Damages => right_Medium_Damages;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        protected virtual void Start()
        {
            forward_Medium_Damages.Add(hit_Forward_Medium_01);
            forward_Medium_Damages.Add(hit_Forward_Medium_02);

            backward_Medium_Damages.Add(hit_Backward_Medium_01);
            backward_Medium_Damages.Add(hit_Backward_Medium_02);

            left_Medium_Damages.Add(hit_Left_Medium_01);
            left_Medium_Damages.Add(hit_Left_Medium_02);

            right_Medium_Damages.Add(hit_Right_Medium_01);
            right_Medium_Damages.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new();

            foreach (var animation in animationList)
            {
                finalList.Add(animation);
            }

            finalList.Remove(lastDamageAnimationPlayed);

            // Check for null entries
            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if (finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting = false)
        {
            float snappedHorizontal;
            float snappedVertical;

            if (horizontalValue > 0f && horizontalValue <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalValue > 0.5f && horizontalValue <= 1)
            {
                snappedHorizontal = 1f;
            }
            else if (horizontalValue < 0 && horizontalValue >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalValue < -0.5f && horizontalValue >= -1)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }

            if (verticalValue > 0f && verticalValue <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalValue > 0.5f && verticalValue <= 1)
            {
                snappedVertical = 1f;
            }
            else if (verticalValue < 0 && verticalValue >= -0.5f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalValue < -0.5f && verticalValue >= -1)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }

            if (isSprinting)
            {
                snappedVertical = 2;
            }

            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false,
            bool isInteracting = false)
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

        public virtual void PlayTargetAttackActionAnimation(
            AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // Keep track of the last attack, for combos
            // Keep track of current attack type (light, heavy, magical)
            // Update animation set to current weapons animations

            character.characterCombatManager.currentAttackType = attackType;
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

        public string LastDamageAnimationPlayed
        {
            get => lastDamageAnimationPlayed;
            set => lastDamageAnimationPlayed = value;
        }
    }
}
