namespace WarehouseAppR.Server.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string msg) : base(msg) { }
    }
}
