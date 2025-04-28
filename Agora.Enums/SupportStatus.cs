
namespace Agora.Enums
{
    public enum SupportStatus
    {
        New,                    // Новое обращение
        InProgress,             // В работе
        AwaitingCustomer,       // Ожидание ответа клиента
        AwaitingSupport,        // Ожидание ответа поддержки
        Resolved,               // Решено
        Closed,                 // Закрыто
        Escalated,              // Передано другому специалисту
        Cancelled,              // Отменено
        Pending,                // На паузе
        Reopened,               // Переоткрыто
        Duplicate               // Дубликат
    }
}
