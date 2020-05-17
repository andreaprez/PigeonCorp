using PigeonCorp.Command;
using PigeonCorp.Dispatcher;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.UserData;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Evolution.UseCase
{
    public class ResetFarmCommand : ICommand
    {
        private readonly EvolutionEntity _evolutionEntity;

        public ResetFarmCommand(EvolutionEntity evolutionEntity)
        {
            _evolutionEntity = evolutionEntity;
        }
        
        public void Execute()
        {
            ResetUserData();
            ReloadScene();
        }

        private void ResetUserData()
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

            _evolutionEntity.CurrentFarmValue.Value = 0f;
        }

        private static void ReloadScene()
        {
            MainDispatcher.Throw();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}