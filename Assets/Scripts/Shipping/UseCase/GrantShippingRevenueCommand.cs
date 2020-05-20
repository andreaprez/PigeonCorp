using PigeonCorp.Command;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.Shipping.Entity;

namespace PigeonCorp.Shipping.UseCase
{
    public class GrantShippingRevenueCommand : ICommand
    {
        private readonly ICommand<float> _addCurrencyCommand;
        private readonly EvolutionEntity _evolutionEntity;
        private readonly ShippingEntity _shippingEntity;

        public GrantShippingRevenueCommand(
            ICommand<float> addCurrencyCommand,
            EvolutionEntity evolutionEntity,
            ShippingEntity shippingEntity
        )
        {
            _addCurrencyCommand = addCurrencyCommand;
            _evolutionEntity = evolutionEntity;
            _shippingEntity = shippingEntity;
        }

        public void Execute()
        {
            var revenue = _evolutionEntity.CurrentEggValue.Value * _shippingEntity.UsedShippingRate.Value;
            _addCurrencyCommand.Execute(revenue);
            
            _evolutionEntity.IncreaseFarmValue(revenue);
        }
    }
}