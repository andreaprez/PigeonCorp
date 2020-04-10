using PigeonCorp.UserState;
using UnityEngine;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarInstaller : MonoBehaviour
    {
        [SerializeField] private MainTopBarView _view;

        public void Install(MainTopBarModel model, UserStateModel userStateModel)
        {
            var mediator = new MainTopBarMediator(model, _view, userStateModel);
        }
    }
}