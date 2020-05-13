using PigeonCorp.Commands;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.ValueModifiers;
using UnityEngine;

namespace PigeonCorp.Research
{
    public class ResearchInstaller: MonoBehaviour
    {
        [SerializeField] private ResearchView _view;

        public void Install(
            ResearchModel model,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand,
            UC_GetResearchValueModifiers getResearchValueModifiersUC,
            UC_GetMainBuyButtonValueModifiers getMainBuyButtonModifiersUC
        )
        {
            // TODO: Use UCs to init Model
            
            var mediator = new ResearchMediator(
                model,
                _view,
                config,
                subtractCurrencyCommand,
                getResearchValueModifiersUC
            );
        }
    }
}