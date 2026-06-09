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
    Preparing = 3,
    Ready = 4,
    Served = 5,
    Cancelled = 6
}

public enum InventoryUnit
{
    Quantity = 1,
    Kilogram = 2,
    Liter = 3,
    Gram = 4,
    Milliliter = 5
}
