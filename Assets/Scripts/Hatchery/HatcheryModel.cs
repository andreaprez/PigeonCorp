namespace Hatchery
{
    public class HatcheryModel
    {
        public readonly bool Built;
        public readonly int MaxCapacity;

        public HatcheryModel()
        {
            // TODO: Get from config
            MaxCapacity = 50;
            Built = true;
        }
    }
}