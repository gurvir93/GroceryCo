using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Application
{
    public class GroceryCoValidator
    {
        public static bool ValidateSelection(string selection, int numOfSelections, out int value)
        {
            if (int.TryParse(selection, out value))
            {
                if (value < 0 || value > numOfSelections - 1)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool ValidateDecimal(string decimalString, out decimal value)
        {
            if (decimal.TryParse(decimalString, out value))
            {
                if (value < 0)
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        public static bool ValidatePercent(string percentString, out int value)
        {
            if (int.TryParse(percentString, out value))
            {
                if (value < 1 || value > 100)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
    }
}
