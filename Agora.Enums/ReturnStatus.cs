
namespace Agora.Enums
{
    public enum ReturnStatus
    {
        Requested,      // Запрос отправлен
        Approved,       // Одобрен
        Rejected,       // Отклонен
        InTransit,      // В пути
        Received,       // Получен
        UnderReview,    // На проверке    
        Cancelled,      // Отменен
        WaitingForPickup, // Ожидание забора
        PartialRefund,  // Частичный возврат    
    }
}
