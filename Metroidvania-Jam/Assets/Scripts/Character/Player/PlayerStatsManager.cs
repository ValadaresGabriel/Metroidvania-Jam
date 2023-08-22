namespace IM
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
            PlayerUIManager.Instace.playerHUDManager.SetMaxHealthValue(newMaxHealth);
        }

        public override void SetMaxStamina(float newMaxStamina)
        {
            base.SetMaxStamina(newMaxStamina);
            PlayerUIManager.Instace.playerHUDManager.SetNewStaminaValue(newMaxStamina);
        }
    }
}
