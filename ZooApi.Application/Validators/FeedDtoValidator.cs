namespace ZooApi.Application.Validators;

public class FeedDtoValidator : AbstractValidator<FeedDto>
{
    public FeedDtoValidator()
    {
        RuleFor(x => x.FoodAmount)
            .InclusiveBetween(1, 100).WithMessage("Порция еды должна быть от 1 до 100");
    }
}
