using PigeonCorp.Dispatcher;
using PigeonCorp.UserState;
using UniRx;

namespace Hatchery
{
    public class HatcheriesMediator
    {
        private readonly HatcheriesModel _model;

        public HatcheriesMediator(
            HatcheriesModel model,
            HatcheriesView view,
            UserStateModel userStateModel
        )
        {
            _model = model;
            
            view.GetOpenButtonAsObservable().Subscribe(open =>
            {
                view.Open();
            }).AddTo(MainDispatcher.Disposables);
            
            view.GetCloseButtonAsObservable().Subscribe(close =>
            {
                view.Close();
            }).AddTo(MainDispatcher.Disposables);
            
            model.UsedCapacity.AsObservable().Subscribe(used =>
            {
                view.UpdateMaxCapacityBar(ComputeCapacityPercentage());
            }).AddTo(MainDispatcher.Disposables);
            
            model.MaxCapacity.AsObservable().Subscribe(max =>
            {
                view.UpdateMaxCapacityText(max);
                view.UpdateMaxCapacityBar(ComputeCapacityPercentage());
            }).AddTo(MainDispatcher.Disposables);
            
            userStateModel.CurrentPigeons.AsObservable()
                .Subscribe(model.UpdateUsedCapacity)
                .AddTo(MainDispatcher.Disposables);
        }

        private float ComputeCapacityPercentage()
        {
            var percentage = (float)_model.UsedCapacity.Value / (float)_model.MaxCapacity.Value;
            return percentage;
        }
    }
}