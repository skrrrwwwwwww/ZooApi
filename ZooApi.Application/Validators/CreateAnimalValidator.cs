namespace ZooApi.Application.Validators;

public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя не может быть пустым")
            .MaximumLength(50).WithMessage("Имя слишком длинное");

        RuleFor(x => x.Species)
            .NotEmpty().WithMessage("Вид должен быть указан");
    }
}