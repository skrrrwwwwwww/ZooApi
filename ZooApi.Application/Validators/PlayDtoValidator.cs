namespace ZooApi.Application.Validators;

public class PlayDtoValidator : AbstractValidator<PlayDto>
{
    public PlayDtoValidator()
    {
        RuleFor(x => x.Intensity)
            .GreaterThan(0).WithMessage("Интенсивность игры должна быть больше 0");
    }
}
