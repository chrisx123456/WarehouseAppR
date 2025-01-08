namespace WarehouseAppR.Server.Exceptions
{
    public class ForbiddenActionPerformedException : Exception 
    {
        public ForbiddenActionPerformedException(string msg) : base(msg) { }

    }
}
