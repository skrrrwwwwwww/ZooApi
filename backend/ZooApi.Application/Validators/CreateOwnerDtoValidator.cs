namespace ZooApi.Application.Validators;

public class CreateOwnerDtoValidator : AbstractValidator<CreateOwnerDto>
{
    public CreateOwnerDtoValidator(IZooDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя владельца обязательно 📄")
            .MinimumLength(2).WithMessage("Имя слишком короткое")
            .MaximumLength(100).WithMessage("Имя слишком длинное");

        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellation) => 
            {
                var exists = await context.Owners.AnyAsync(o => o.Name == name, cancellation);
                return !exists; 
            })
            .WithMessage("Владелец с таким именем уже зарегистрирован в зоопарке 🦁");
    }
}