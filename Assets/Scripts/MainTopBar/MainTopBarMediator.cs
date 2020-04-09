using PigeonCorp.UserState;
using UniRx;

namespace PigeonCorp.MainTopBar
{
    public class MainTopBarMediator
    {
        private MainTopBarModel _model;
        private MainTopBarView _view;

        public MainTopBarMediator(
            MainTopBarModel model,
            MainTopBarView view,
            UserStateModel userStateModel
        )
        {
            _model = model;
            _view = view;

            userStateModel.Currency.AsObservable().Subscribe(_model.SetCurrency);
            userStateModel.CurrentPigeons.AsObservable().Subscribe(_model.SetPigeonsCount);
            
            _model.Currency.AsObservable().Subscribe(_view.UpdateCurrencyText);
            _model.PigeonsCount.AsObservable().Subscribe(_view.UpdatePigeonsCountText);
        }
    }
}