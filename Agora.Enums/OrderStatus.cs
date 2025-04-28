
using System.Reflection;

namespace Agora.Enums
{
    public enum OrderStatus
    {
        Pending,        // Ожидает обработки
        Processing,     // В обработке
        Shipped,        // Отправлен
        Delivered,      // Доставлен
        Cancelled,      // Отменен
        Refunded,       // Возвращен
        Completed       // Завершен
    }

}
