namespace ZooApi.Application.Validators;

public class OwnerUpdateInfoValidator : AbstractValidator<OwnerUpdateInfo>
{
    public OwnerUpdateInfoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя владельца обязательно 📄")
            .MinimumLength(2).WithMessage("Имя слишком короткое")
            .MaximumLength(100).WithMessage("Имя слишком длинное");

        RuleFor(x => x.ContactInfo)
            .NotEmpty().WithMessage("Контактная информация обязательна")
            .MaximumLength(500).WithMessage("Контактная информация слишком длинная");
    }
}
