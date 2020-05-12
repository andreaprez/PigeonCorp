using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.ValueModifiers;
using UnityEngine;

namespace PigeonCorp.Research
{
    public class ResearchInstaller: MonoBehaviour
    {
        [SerializeField] private ResearchView _view;

        public void Install(ResearchModel model,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUc
        )
        {
            // TODO: Use UC to init Model
            
            var mediator = new ResearchMediator(
                model,
                _view,
                config,
                subtractCurrencyCommand
            );
        }
    }
}