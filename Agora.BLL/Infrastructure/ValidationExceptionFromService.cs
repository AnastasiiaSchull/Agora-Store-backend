
namespace Agora.BLL.Infrastructure
{
    public class ValidationExceptionFromService: Exception
    {
        public string Property { get; protected set; }
        public ValidationExceptionFromService(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
