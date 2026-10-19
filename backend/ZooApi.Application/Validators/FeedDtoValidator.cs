namespace ZooApi.Application.Validators;

public class FeedDtoValidator : AbstractValidator<FeedDto>
{
    public FeedDtoValidator()
    {
        RuleFor(x => x.FoodAmount)
            .InclusiveBetween(1, 100)
            .WithMessage("Количество еды должно быть в диапазоне 1-100");
    }
}
