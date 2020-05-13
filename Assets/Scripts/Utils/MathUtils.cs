namespace PigeonCorp.Utils
{
    public class MathUtils
    {
        public static float CalculatePercentageDecimalFromQuantity(float quantity, float total)
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
        
        public static float CalculateQuantityFromPercentage(float percentage, float total)
        {
            var quantity = percentage * total / 100f;
            return quantity;
        }
    }
}