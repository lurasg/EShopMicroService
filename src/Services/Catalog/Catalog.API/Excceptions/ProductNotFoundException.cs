using BuildingBlocks.Exceptions;

namespace Catalog.API.Excceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }
    }
}
