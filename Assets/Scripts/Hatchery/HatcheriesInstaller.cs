using PigeonCorp.UserState;
using UnityEngine;

namespace Hatchery
{
    public class HatcheriesInstaller : MonoBehaviour
    {
        [SerializeField] private HatcheriesView _view;

        public void Install(HatcheriesModel model, UserStateModel userStateModel)
        {
            var mediator = new HatcheriesMediator(model, _view, userStateModel);
        }
    }
}