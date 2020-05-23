using System;

namespace PigeonCorp.Utils
{
    public class DisplayableNumber
    {
        public const string THOUSAND_SEPARATOR = "{0:0,0}";
        public const string THOUSAND_SEPARATOR_WITH_ONE_DECIMAL = "{0:0,0.#}";

        public static string Parse(string format, float number)
        {
            var formatted = String.Format(format, number);
            formatted = formatted.TrimStart('0');

            return formatted;
        }
    }
}