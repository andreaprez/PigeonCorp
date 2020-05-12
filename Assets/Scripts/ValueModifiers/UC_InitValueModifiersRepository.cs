namespace PigeonCorp.ValueModifiers
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
            var mainBuyButton = new BuyButtonValueModifiers();
            _repository.Add(mainBuyButton);
        }
    }
}