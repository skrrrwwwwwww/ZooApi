namespace ZooApi.Application.Validators;

public class PlayDtoValidator : AbstractValidator<PlayDto>
{
    public PlayDtoValidator()
    {
        RuleFor(x => x.Intensity)
            .InclusiveBetween(1, Animal.MaxIntensity)
            .WithMessage($"Интенсивность должна быть в диапазоне 1-{Animal.MaxIntensity}");
    }
}
