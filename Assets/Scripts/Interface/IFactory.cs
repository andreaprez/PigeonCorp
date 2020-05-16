namespace PigeonCorp.Factory
{
    public interface IFactory
    {
        void Create();
    }
    
    public interface IFactory<in TParam>
    {
        void Create(TParam param1);
    }
    
    public interface IFactory<in TParam1, in TParam2>
    {
        void Create(TParam1 param1, TParam2 param2);
    }

}