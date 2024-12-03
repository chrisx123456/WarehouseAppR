using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.DataAnnotations
{
    public class Ean : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var stringValue = value as string;
            return stringValue != null && (stringValue.Length == 8 || stringValue.Length == 13) && IsDigitsOnly(stringValue);
        }
        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
