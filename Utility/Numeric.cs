using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// Numeric:  This class contains functionality specific to using numeric values
    /// in the C# language.  This is functionality not currently built into the C# language.
    /// </summary>
    public static class Numeric
    {
        /// <summary>
        /// IsNumeric:  A method that evaluates the String parameter against
        /// the NumberStyles parameter to determine whether or not the String
        /// can be safely converted to its corresponding numeric type.
        /// </summary>
        /// <param name="value">The value being evaluated.</param>
        /// <param name="numberStyles">The specific number style used in the evaluation.</param>
        /// <returns></returns>
        public static bool IsNumeric(String value, NumberStyles numberStyle)
        {
            //result variable not used but required in method parameters.
            double result;
            //Attempt to parse the string argument to a numeric based on numberstyle given
            //and the regional settings (culture info) on the computer in use.
            return Double.TryParse(value, numberStyle, CultureInfo.CurrentCulture, out result);

            //usage:
            //if (isNumeric(Value, System.Globalization.NumberStyles.Float))
            //{
            //    //code for numeric data here
            //}

        }

        /// <summary>
        /// ClearFormatting:  A method that receives a String containing special characters
        /// and returns the same string without any of the special characters.  
        /// For example:  The formatted value may be "123-456-789", the format may be "-"
        ///               The returned value would be "123456789"
        /// </summary>
        /// <param name="formatted">The formatted string.</param>
        /// <param name="format">The string to be removed from the formatted string.</param>
        /// <returns></returns>
        public static String ClearFormatting(String formatted, String format)
        {
            //Return string without format character(s).
            return formatted.Replace(format, "");
        }
    }
}
