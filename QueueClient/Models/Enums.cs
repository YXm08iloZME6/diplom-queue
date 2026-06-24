namespace QueueClient.Models;

// Копии enum'ов из Domain.Enums. API сериализует их числами (значения по порядку),
// поэтому важен именно порядок объявления — он совпадает с серверным.

public enum TicketStatus { Waiting, Called, Processing, Completed, Cancelled }

public enum UserStatus { Waiting, Away, Busy }

public enum WindowStatus { Open, Close, Busy, Offline }

public enum TypeOfSettingsValue { Bool, Int, String }
