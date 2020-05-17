using PigeonCorp.ValueModifiers.Entity;

namespace PigeonCorp.ValueModifiers.UseCase
{
    public class UC_GetEvolutionValueModifiers
    {
        private readonly ValueModifiersRepository _repository;

        public UC_GetEvolutionValueModifiers(ValueModifiersRepository repository)
        {
            _repository = repository;
        }
        
        public BaseValueModifiers Execute()
        {
            return _repository.Get<EvolutionValueModifiers>();
        }
    }
}