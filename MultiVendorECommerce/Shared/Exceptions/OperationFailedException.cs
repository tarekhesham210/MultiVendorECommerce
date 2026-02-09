namespace MultiVendorECommerce.Exceptions
{
    public class OperationFailedException : DomainException
    {
        public OperationFailedException(string message) : base(message) { }
       
    }

}
