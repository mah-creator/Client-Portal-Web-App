using System.Globalization;

namespace ClientPortalApi.Utils;

public static class Money
{
    public static string FormatPrice(decimal price, string currency)
    {
        // Normalize the currency code to uppercase for case insensitivity
        string normalizedCurrency = currency.ToUpper();
        
        // Get culture info based on currency
        CultureInfo culture = normalizedCurrency switch
        {
            "USD" => new CultureInfo("en-US"),      // US Dollar
            "EUR" => new CultureInfo("de-DE"),      // Euro (German format)
            "GBP" => new CultureInfo("en-GB"),      // British Pound
            "JPY" => new CultureInfo("ja-JP"),      // Japanese Yen
            "CAD" => new CultureInfo("en-CA"),      // Canadian Dollar
            "AUD" => new CultureInfo("en-AU"),      // Australian Dollar
            _ => new CultureInfo("en-US")           // Default to US format
        };

        // Clone the NumberFormat to avoid modifying the original
        var numberFormat = (NumberFormatInfo)culture.NumberFormat.Clone();
        
        // Set fraction digits to match your JavaScript function behavior
        numberFormat.CurrencyDecimalDigits = (price % 1 == 0) ? 0 : 2;
        
        return price.ToString("C", numberFormat);
    }
}