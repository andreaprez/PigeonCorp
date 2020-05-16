using PigeonCorp.ValueModifiers.Entity;

namespace PigeonCorp.ValueModifiers.UseCase
{
    public class UC_InitValueModifiersRepository
    {
        ValueModifiersRepository _repository;
        
        public ValueModifiersRepository Execute()
        {
            _repository = new ValueModifiersRepository();
            
            FillRepository();

            return _repository;
        }

        private void FillRepository()
        {
            var mainBuyButton = new MainBuyButtonValueModifiers();
            _repository.Add(mainBuyButton);
            
            var hatcheries = new HatcheriesValueModifiers();
            _repository.Add(hatcheries);
            
            var shipping = new ShippingValueModifiers();
            _repository.Add(shipping);
            
            var research = new ResearchValueModifiers();
            _repository.Add(research);
        }
    }
}