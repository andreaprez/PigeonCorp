using PigeonCorp.Shipping;

namespace PigeonCorp.Commands
{
    public class GrantShippingRevenueCommand : ICommand
    {
        private readonly ICommand<float> _addCurrencyCommand;
        // private readonly EvolutionModel _evolutionModel;
        private readonly ShippingModel _shippingModel;

        public GrantShippingRevenueCommand(
            ICommand<float> addCurrencyCommand,
            //EvolutionModel evolutionModel,
            ShippingModel shippingModel
        )
        {
            _addCurrencyCommand = addCurrencyCommand;
            //_evolutionModel = evolutionModel;
            _shippingModel = shippingModel;
        }

        public void Execute()
        {
            // var revenue = _evolutionModel.EggValue * _shippingModel.UsedShippingRate.Value;
            var revenue = 1 * _shippingModel.UsedShippingRate.Value;
            _addCurrencyCommand.Execute(revenue);
        }
    }
}