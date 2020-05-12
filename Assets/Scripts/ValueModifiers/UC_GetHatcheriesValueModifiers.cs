namespace PigeonCorp.ValueModifiers
{
    public class UC_GetHatcheriesValueModifiers
    {
        private readonly ValueModifiersRepository _repository;

        public UC_GetHatcheriesValueModifiers(ValueModifiersRepository repository)
        {
            _repository = repository;
        }
        
        public BaseValueModifiers Execute()
        {
            return _repository.Get<HatcheriesValueModifiers>();
        }
    }
}