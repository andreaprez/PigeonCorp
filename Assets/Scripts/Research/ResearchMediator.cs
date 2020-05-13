using System;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using PigeonCorp.UserState;
using PigeonCorp.ValueModifiers;
using UniRx;

namespace PigeonCorp.Research
{
    public class ResearchMediator
    {
        private readonly ResearchModel _model;
        private readonly ResearchView _view;
        private readonly ResearchTitleData _config;
        private readonly ICommand<float> _subtractCurrencyCommand;
        private readonly ResearchValueModifiers _valueModifiers;
        private readonly UserStateModel _userStateModel;


        public ResearchMediator(
            ResearchModel model,
            ResearchView view,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand,
            UC_GetResearchValueModifiers getResearchValueModifiersUc,
            UserStateModel userStateModel
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            _userStateModel = userStateModel;

            _valueModifiers = (ResearchValueModifiers)getResearchValueModifiersUc.Execute();
            
            view.GetOpenButtonAsObservable().Subscribe(open =>
            {
                view.Open();
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetCloseButtonAsObservable().Subscribe(close =>
            {
                view.Close();
            }).AddTo(MainDispatcher.Disposables);
            
            InitializeSubViews();
        }
        
        private void InitializeSubViews()
        {
            for (int i = 0; i < _model.Bonuses.Count; i++)
            {
                var bonusMediator = new BonusMediator(
                    _model,
                    _model.Bonuses[i],
                    _view.GetBonusView(i),
                    _config,
                    _subtractCurrencyCommand,
                    _valueModifiers,
                    _userStateModel
                );
            }
        }
    }
}