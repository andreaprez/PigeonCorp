using System.Collections.Generic;
using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.Hatchery
{
    public class HatcheriesInstaller : MonoBehaviour
    {
        [SerializeField] private HatcheriesView _view;
        [SerializeField] private List<GameObject> _hatcheryPrefabs;

        public void Install(
            HatcheriesModel model,
            HatcheriesTitleData config,
            UserStateModel userStateModel,
            ICommand<float> subtractCurrencyCommand
        )
        {
            var mediator = new HatcheriesMediator(
                model,
                _view,
                config,
                userStateModel,
                subtractCurrencyCommand,
                _hatcheryPrefabs
            );
        }
    }
}