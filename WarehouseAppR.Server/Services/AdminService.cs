using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class AdminService : IAdminService
    {
        public CurrencyDTO GetCurrency()
        {
            string json = File.ReadAllText("settings.json");
            JsonNode? jn = JsonNode.Parse(json);
            string? currnecy = jn?["CurrencyName"].GetValue<string>();
            if (currnecy == null || !currnecy.Any())
                throw new NotFoundException("Currency not found");
            return new CurrencyDTO { Currency = currnecy };
        }
        public void SetCurrency(string currency)
        {
            string json = File.ReadAllText("settings.json");
            JsonNode? jn = JsonNode.Parse(json);
            jn!["CurrencyName"] = currency;
            File.WriteAllText("settings.json", jn?.ToJsonString());
        }
    }
}
