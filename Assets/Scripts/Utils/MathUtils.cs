namespace PigeonCorp.Utils
{
    public class MathUtils
    {
        public static float CalculatePercentage(float quantity, float total)
        {
            var percentage = 0f;
            
            if (quantity > total)
            {
                percentage = 1f;
            }
            
            else if (quantity > 0 && total > 0)
            {
                percentage = quantity / total;
            }
            
            return percentage;
        }
    }
}