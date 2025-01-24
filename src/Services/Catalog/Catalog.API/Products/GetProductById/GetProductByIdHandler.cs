

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuey(Guid Id) : IQuery<GetProductByIdResult>;
public record class GetProductByIdResult(Product Product);
internal class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuey, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuey query, CancellationToken cancellationToken)
    {
       var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        return new GetProductByIdResult(product);
    }
}

