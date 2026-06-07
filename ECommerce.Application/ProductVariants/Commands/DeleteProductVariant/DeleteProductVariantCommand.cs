using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.DeleteProductVariant
{
    public class DeleteProductVariantCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteProductVariantCommand(int id)
        {
            Id = id;
        }
    }
}
