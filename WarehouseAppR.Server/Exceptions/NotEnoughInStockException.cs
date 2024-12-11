namespace WarehouseAppR.Server.Exceptions
{
    public class NotEnoughInStockException : Exception
    {
        public NotEnoughInStockException(string msg) : base(msg) { }
    }

}
