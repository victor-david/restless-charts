using Restless.Controls.Chart;

namespace Application.Sample
{
    public class DoubleToLookupConverter : IDoubleConverter
    {
        /// <summary>
        /// Converts the specified value into a string via a table lookup.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="format">The format (in this case, ignored)</param>
        /// <returns>A string</returns>
        public string Convert(double value, string format)
        {
            long id = (long)value;
            // Normally, we'd use the id to lookup a corresponding category (for instance)
            // in a table or other data structure. For this sample, we fake it.

            switch (id)
            {
                case 1:
                    return "Art";
                case 2:
                    return "Food";
                case 3:
                    return "Home";
                case 4:
                    return "Auto";
                case 5:
                    return "Taxes";
                case 6:
                    return "Gas";
                case 7:
                    return "Water";
                case 8:
                    return "Rent";
                case 9:
                    return "Atm";
                case 10:
                    return "Electric";
                case 11:
                    return "Cats";
                case 12:
                    return "Dogs";
                case 13:
                    return "Computer";
                case 14:
                    return "TV";
                default:
                    return $"Cat {id}";
            }
        }
    }
}
