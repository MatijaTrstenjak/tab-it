namespace tab_it.Models.Domain;

public enum TabStatus
{
    Open = 1,
    Closed = 2,
    Cancelled = 3
}

public enum OrderStatus
{
    Draft = 1,
    SentToBar = 2,
    Ready = 3,
    Served = 4,
    Cancelled = 5
}
