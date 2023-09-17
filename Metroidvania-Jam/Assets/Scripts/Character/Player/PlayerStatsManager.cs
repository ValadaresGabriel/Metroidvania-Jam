namespace TS
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        protected override void Start()
        {
            base.Start();
        }

        public override void InitializeStats()
        {
            base.InitializeStats();
        }

        public override void SetMaxHealth(float newMaxHealth)
        {
            base.SetMaxHealth(newMaxHealth);
            PlayerUIManager.Instance.playerHUDManager.SetMaxHealthValue(newMaxHealth);
        }

        public override void SetMaxStamina(float newMaxStamina)
        {
            base.SetMaxStamina(newMaxStamina);
            PlayerUIManager.Instance.playerHUDManager.SetNewStaminaValue(newMaxStamina);
        }
    }
}
