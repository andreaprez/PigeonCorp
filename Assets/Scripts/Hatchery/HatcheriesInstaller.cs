using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Factory;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.Research;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.Hatcheries
{
    public class HatcheriesInstaller : MonoBehaviour
    {
        [SerializeField] private HatcheriesView _view;
        [Space]
        [SerializeField] private List<GameObject> _hatcheryPrefabs;
        [SerializeField] private List<Transform> _hatcheryContainers;

        public void Install(
            HatcheriesModel model,
            HatcheriesTitleData config,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand,
            ResearchModel researchModel
        )
        {
            var hatcheryFactory = new HatcheryFactory(
                _hatcheryPrefabs,
                _hatcheryContainers
            );
            var spawnHatcheryCommand = new SpawnHatcheryCommand(hatcheryFactory);
            
            var mediator = new HatcheriesMediator(
                model,
                _view,
                config,
                userStateModel,
                subtractCurrencyCommand,
                spawnHatcheryCommand,
                _hatcheryContainers,
                researchModel
            );
        }
    }
}