using FluentValidation.TestHelper;
using ZooApi.Application.DTOs;
using ZooApi.Application.Validators;
using ZooApi.Domain.Entities;
using ZooApi.Domain.Exceptions;

namespace ZooApi.Tests;

public class DomainAndValidationTests
{
    [Fact]
    public void Feed_RejectsAmountsOutsideSupportedRange()
    {
        var animal = new Animal("Leo", "Lion", Guid.NewGuid());

        Assert.Throws<ArgumentOutOfRangeException>(() => animal.Feed(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => animal.Feed(101));
    }

    [Fact]
    public void Feed_CapsEnergyAtOneHundred()
    {
        var animal = new Animal("Leo", "Lion", Guid.NewGuid());
        animal.Feed(100);

        Assert.Equal(100, animal.Energy);
    }

    [Fact]
    public void Play_ConsumesEnergyAndStoresIntensity()
    {
        var animal = new Animal("Leo", "Lion", Guid.NewGuid());
        animal.Play(20);

        Assert.Equal(60, animal.Energy);
        Assert.Equal(20, animal.Intensity);
    }

    [Fact]
    public void Play_RejectsIntensityOutsideSupportedRange()
    {
        var animal = new Animal("Leo", "Lion", Guid.NewGuid());

        Assert.Throws<ArgumentOutOfRangeException>(() => animal.Play(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => animal.Play(Animal.MaxIntensity + 1));
    }

    [Fact]
    public void Play_RejectsIntensityThatExceedsRemainingEnergy()
    {
        var animal = new Animal("Leo", "Lion", Guid.NewGuid());
        animal.Play(40);

        Assert.Throws<AnimalTooTiredException>(() => animal.Play(11));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void FeedValidator_RejectsOutOfRangeAmounts(int amount)
    {
        var result = new FeedDtoValidator().TestValidate(new FeedDto(amount));

        result.ShouldHaveValidationErrorFor(dto => dto.FoodAmount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void FeedValidator_AcceptsRangeBoundaries(int amount)
    {
        var result = new FeedDtoValidator().TestValidate(new FeedDto(amount));

        result.ShouldNotHaveValidationErrorFor(dto => dto.FoodAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void PlayValidator_RejectsOutOfRangeIntensity(int intensity)
    {
        var result = new PlayDtoValidator().TestValidate(new PlayDto(intensity));

        result.ShouldHaveValidationErrorFor(dto => dto.Intensity);
    }

    [Fact]
    public void OwnerUpdateInfoValidator_RejectsEmptyNameAndContactInfo()
    {
        var result = new OwnerUpdateInfoValidator().TestValidate(new OwnerUpdateInfo("", ""));

        result.ShouldHaveValidationErrorFor(dto => dto.Name);
        result.ShouldHaveValidationErrorFor(dto => dto.ContactInfo);
    }

    [Fact]
    public void OwnerUpdateInfoValidator_AcceptsValidDetails()
    {
        var result = new OwnerUpdateInfoValidator()
            .TestValidate(new OwnerUpdateInfo("Alex", "alex@example.com"));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void OwnerUpdateDetails_ChangesNameAndContactInfo()
    {
        var owner = new Owner("Alex", "old@example.com");
        owner.UpdateDetails("Taylor", "new@example.com");

        Assert.Equal("Taylor", owner.Name);
        Assert.Equal("new@example.com", owner.ContactInfo);
    }
}
