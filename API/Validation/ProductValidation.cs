using API.DTOs;
using API.Entities;
using FluentValidation;

namespace API.Validation
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

        }
    }
}
