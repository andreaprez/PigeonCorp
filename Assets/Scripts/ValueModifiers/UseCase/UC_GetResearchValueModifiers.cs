using PigeonCorp.ValueModifiers.Entity;

namespace PigeonCorp.ValueModifiers.UseCase
{
    public class UC_GetResearchValueModifiers
    {
        private readonly ValueModifiersRepository _repository;

        public UC_GetResearchValueModifiers(ValueModifiersRepository repository)
        {
            _repository = repository;
        }
        
        public BaseValueModifiers Execute()
        {
            return _repository.Get<ResearchValueModifiers>();
        }
    }
}