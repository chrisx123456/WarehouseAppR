using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.DataAnnotations
{
    public class StringLength813 : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var stringValue = value as string;
            return stringValue != null && (stringValue.Length == 8 || stringValue.Length == 13);
        }
    }
}
