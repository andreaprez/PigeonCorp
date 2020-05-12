using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.Commands
{
    public class InitializeUserCommand : ICommand
    {
        public void Execute()
        {
            var userStateConfig = Gateway.Instance.GetUserStateConfig();
            var userStateData = new UserStateUserData(userStateConfig);
            Gateway.Instance.UpdateUserStateData(userStateData);

            var hatcheriesConfig = Gateway.Instance.GetHatcheriesConfig();
            var hatcheriesData = new HatcheriesUserData(hatcheriesConfig);
            Gateway.Instance.UpdateHatcheriesData(hatcheriesData);
            
            var shippingConfig = Gateway.Instance.GetShippingConfig();
            var shippingData = new ShippingUserData(shippingConfig);
            Gateway.Instance.UpdateShippingData(shippingData);
            
            var researchConfig = Gateway.Instance.GetResearchConfig();
            var researchData = new ResearchUserData(researchConfig);
            Gateway.Instance.UpdateResearchData(researchData);
            
            var userInitData = new UserInitializedUserData(true);
            Gateway.Instance.UpdateUserInitialized(userInitData);
        }
    }
}