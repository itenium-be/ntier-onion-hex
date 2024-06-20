namespace Acerola.Infrastructure
{
    using System;
    public class InfrastructureException : ApplicationException
    {
        internal InfrastructureException(string businessMessage)
               : base(businessMessage)
        {
        }
    }
}
