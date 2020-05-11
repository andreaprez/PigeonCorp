using System;
using PigeonCorp.Commands;
using PigeonCorp.Dispatcher;
using PigeonCorp.Persistence.Gateway;
using PigeonCorp.Persistence.TitleData;
using UniRx;

namespace PigeonCorp.Research
{
    public class ResearchMediator
    {
        private readonly ResearchModel _model;
        private readonly ResearchView _view;
        private readonly ResearchTitleData _config;
        private readonly ICommand<float> _subtractCurrencyCommand;
        
        public ResearchMediator(
            ResearchModel model,
            ResearchView view,
            ResearchTitleData config,
            ICommand<float> subtractCurrencyCommand
        )
        {
            _model = model;
            _view = view;
            _config = config;
            _subtractCurrencyCommand = subtractCurrencyCommand;
            
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
                    _subtractCurrencyCommand
                );
            }
        }
    }
}