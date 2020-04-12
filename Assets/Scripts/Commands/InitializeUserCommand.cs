using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;

namespace PigeonCorp.Commands
{
    public class InitializeUserCommand : ICommand
    {
        public void Handle()
        {
            var userStateConfig = Gateway.Instance.GetUserStateConfig();
            var userStateData = new UserStateUserData(userStateConfig);
            Gateway.Instance.UpdateUserStateData(userStateData);

            var hatcheriesConfig = Gateway.Instance.GetHatcheriesConfig();
            var hatcheriesData = new HatcheriesUserData(hatcheriesConfig);
            Gateway.Instance.UpdateHatcheriesData(hatcheriesData);
            
            
            var userInitData = new UserInitializedUserData(true);
            Gateway.Instance.UpdateUserInitialized(userInitData);
        }
    }
}