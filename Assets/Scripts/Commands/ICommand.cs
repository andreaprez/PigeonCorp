namespace PigeonCorp.Commands
{
    public interface ICommand
    {
        void Handle();
    }

    public interface ICommand<in TParam>
    {
        void Handle(TParam param);
    }

    public interface ICommand<in TParam1, in TParam2>
    {
        void Handle(TParam1 param1, TParam2 param2);
    }
}