
namespace Agora.Enums
{
    public enum ShippingStatus
    {
        Pending,                // Ожидает отправки
        InTransit,              // В пути
        InfoReceived,           // Получены данные о посылке
        PickedUp,               // Забрана курьером
        ArrivedAtFacility,      // Прибыла на склад
        DepartedFacility,       // Покинула склад
        ProcessedAtExport,      // На экспортной обработке
        CustomsInProgress,      // Проходит таможню
        CustomsCleared,         // Таможня пройдена
        OutForDelivery,         // Курьер в пути к получателю
        DeliveryAttempted,      // Попытка доставки
        Delivered,              // Доставлено
        Failed,                 // Ошибка доставки
        ReturnedToSender,       // Возврат отправителю
        OnHold,                 // Ожидает (задержка)
        Delayed                 // Задержана
    }

}
