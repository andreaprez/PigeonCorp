using System;
using PigeonCorp.Command;
using PigeonCorp.Evolution.Entity;
using PigeonCorp.MainTopBar.Entity;
using PigeonCorp.Shipping.Entity;

namespace PigeonCorp.GameInstallation
{
    public class GrantOfflineRevenueCommand : ICommand
    {
        private readonly ICommand<float> _addCurrencyCommand;
        private readonly EvolutionEntity _evolutionEntity;
        private readonly ShippingEntity _shippingEntity;
        private readonly MainTopBarEntity _mainTopBarEntity;

        public GrantOfflineRevenueCommand(
            ICommand<float> addCurrencyCommand,
            EvolutionEntity evolutionEntity,
            ShippingEntity shippingEntity,
            MainTopBarEntity mainTopBarEntity
        )
        {
            _addCurrencyCommand = addCurrencyCommand;
            _evolutionEntity = evolutionEntity;
            _shippingEntity = shippingEntity;
            _mainTopBarEntity = mainTopBarEntity;
        }

        public void Execute()
        {
            var revenuePerMinute = _evolutionEntity.CurrentEggValue.Value * _shippingEntity.UsedShippingRate.Value;
            var revenuePerHour = revenuePerMinute * 60;
            
            var elapsedTimeTicks = DateTime.Now.Ticks - _mainTopBarEntity.LastTimeOnline;
            var elapsedTimeHours = (float)TimeSpan.FromTicks(elapsedTimeTicks).TotalHours;
            elapsedTimeHours = elapsedTimeHours > 2 ? 2 : elapsedTimeHours;

            var totalRevenue = elapsedTimeHours * revenuePerHour;

            _addCurrencyCommand.Execute(totalRevenue);
            _evolutionEntity.IncreaseFarmValue(totalRevenue);
        }
    }
}