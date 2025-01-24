
namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuey(string Category) : IQuery<GetProductByCategoryResult>;
public record class GetProductByCategoryResult(IEnumerable<Product> Products);
internal class GetProductByCategoryQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuey, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuey query, CancellationToken cancellationToken)
    {
       var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}
