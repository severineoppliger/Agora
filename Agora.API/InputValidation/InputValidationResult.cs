namespace Agora.API.InputValidation;

/* Object describing the result of an input validation, with all input validation errors */
public class InputValidationResult
{
    public List<string> Errors { get; } = [];

    public bool IsValid => !Errors.Any();
}