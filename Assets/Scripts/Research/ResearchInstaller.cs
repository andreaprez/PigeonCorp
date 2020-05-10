using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using UnityEngine;

namespace PigeonCorp.Research
{
    public class ResearchInstaller: MonoBehaviour
    {
        [SerializeField] private ResearchView _view;

        public void Install(
            ResearchModel model,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand
        )
        {

            var mediator = new ResearchMediator(
                model,
                _view,
                config,
                subtractCurrencyCommand
            );
        }
    }
}