using PigeonCorp.ValueModifiers.Entity;

namespace PigeonCorp.ValueModifiers.UseCase
{
    public class UC_GetMainBuyButtonValueModifiers
    {
        private readonly ValueModifiersRepository _repository;

        public UC_GetMainBuyButtonValueModifiers(ValueModifiersRepository repository)
        {
            _repository = repository;
        }
        
        public BaseValueModifiers Execute()
        {
            return _repository.Get<MainBuyButtonValueModifiers>();
        }
    }
}