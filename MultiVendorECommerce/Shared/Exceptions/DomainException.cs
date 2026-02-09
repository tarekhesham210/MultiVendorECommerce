namespace MultiVendorECommerce.Exceptions
{
    public abstract class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }

        //all exceptions that are related to the domain should inherit from this class
    }

}
