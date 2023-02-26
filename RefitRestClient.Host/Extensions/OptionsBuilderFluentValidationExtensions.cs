using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace RefitRestClient.Host.Extensions;

public static class OptionsBuilderFluentValidationExtensions
{
    /// <summary>
    ///     Register this options instance for validation of its DataAnnotations.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="optionsBuilder">The options builder to add the services to.</param>
    /// <returns>The <see cref="OptionsBuilder{TOptions}" /> so that additional calls can be chained.</returns>
    [RequiresUnreferencedCode(
        "Uses DataAnnotationValidateOptions which is unsafe given that the options type passed in when calling Validate cannot be statically analyzed so its" +
        " members may be trimmed.")]
    public static OptionsBuilder<TOptions> ValidateFluently<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties |
                                    DynamicallyAccessedMemberTypes.NonPublicProperties)]
        TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(services =>
            new FluentValidateOptions<TOptions>(optionsBuilder.Name,
                services.GetRequiredService<IValidator<TOptions>>()));
        return optionsBuilder;
    }
}

/// <summary>
///     Implementation of <see cref="IValidateOptions{TOptions}" /> that uses DataAnnotation's <see cref="Validator" /> for
///     validation.
/// </summary>
/// <typeparam name="TOptions">The instance being validated.</typeparam>
public class FluentValidateOptions<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties |
                                    DynamicallyAccessedMemberTypes.NonPublicProperties)]
        TOptions>
    : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="validator">The particular options validator</param>
    [RequiresUnreferencedCode(
        "The implementation of Validate method on this type will walk through all properties of the passed in options object, and its type cannot be " +
        "statically analyzed so its members may be trimmed.")]
    public FluentValidateOptions(string? name, IValidator<TOptions> validator)
    {
        _validator = validator;
        Name = name;
    }

    /// <summary>
    ///     The options name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    ///     Validates a specific named options instance (or all when <paramref name="name" /> is null).
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance.</param>
    /// <returns>The <see cref="ValidateOptionsResult" /> result.</returns>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode",
        Justification =
            "Suppressing the warnings on this method since the constructor of the type is annotated as RequiresUnreferencedCode.")]
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (Name != null && Name != name)
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;

        // Ensure options are provided to validate against
        if (options == null) throw new ArgumentNullException(nameof(options));

        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid) return ValidateOptionsResult.Success;

        var errors = validationResult.Errors.Select(failure =>
            $"Options validation failed for '{failure.PropertyName}' with error: '{failure.ErrorMessage}'.");

        return ValidateOptionsResult.Fail(errors);
    }
}
