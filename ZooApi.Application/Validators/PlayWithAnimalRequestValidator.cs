namespace ZooApi.Application.Validators;

public class PlayWithAnimalRequestValidator : AbstractValidator<PlayDto>
{
    public PlayWithAnimalRequestValidator()
    {
        RuleFor(x => x.Intensity)
            .GreaterThan(0).WithMessage("Интенсивность игры должна быть больше 0");
    }
}
