using PigeonCorp.Command;
using PigeonCorp.Shipping.Entity;

namespace PigeonCorp.Shipping.UseCase
{
    public class GrantShippingRevenueCommand : ICommand
    {
        private readonly ICommand<float> _addCurrencyCommand;
        // private readonly EvolutionModel _evolutionModel;
        private readonly ShippingEntity _shippingEntity;

        public GrantShippingRevenueCommand(
            ICommand<float> addCurrencyCommand,
            //EvolutionModel evolutionModel,
            ShippingEntity shippingEntity
        )
        {
            _addCurrencyCommand = addCurrencyCommand;
            //_evolutionModel = evolutionModel;
            _shippingEntity = shippingEntity;
        }

        public void Execute()
        {
            // var revenue = _evolutionModel.EggValue * _shippingModel.UsedShippingRate.Value;
            var revenue = 1 * _shippingEntity.UsedShippingRate.Value;
            _addCurrencyCommand.Execute(revenue);
        }
    }
}