namespace ZooApi.Application.Validators;

public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalDtoValidator(IZooDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя льва не может быть пустым 🦁")
            .MaximumLength(50).WithMessage("Слишком длинное имя");

        RuleFor(x => x.Species)
            .NotEmpty().WithMessage("Нужно указать вид животного");

        // А вот та самая проверка, из-за которой мы мучались 2 дня:
        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .MustAsync(async (ownerId, cancellation) => 
            {
                // Проверяем существование владельца прямо при валидации DTO
                return await context.Owners.AnyAsync(o => o.Id == ownerId, cancellation);
            })
            .WithMessage("Владелец с таким ID не найден. Сначала создайте владельца! 📄");
    }
}