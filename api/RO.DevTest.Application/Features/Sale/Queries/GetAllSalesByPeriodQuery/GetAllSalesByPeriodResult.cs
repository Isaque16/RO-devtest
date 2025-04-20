namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

public record GetAllSalesByPeriodResult(
    int TotalSalesCount,
    decimal TotalRevenue,
    List<ProductRevenue> ProductRevenues
);

public record ProductRevenue(
    Guid ProductId,
    string ProductName,
    decimal TotalRevenue
);
