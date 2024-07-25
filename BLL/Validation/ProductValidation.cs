using BLL.DTOs;
using FluentValidation;

namespace BLL.Validation
{
    public class ProductValidation : AbstractValidator<CreateUpdateProductDTO>
    {
        public ProductValidation()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be greater than zero");


        }
    }
}
