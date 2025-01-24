namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProdcutResult>;

public record DeleteProdcutResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProdcutResult>
{
    public async Task<DeleteProdcutResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {

        session.Delete<Product>(command.Id);

        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProdcutResult(true);
    }
}

