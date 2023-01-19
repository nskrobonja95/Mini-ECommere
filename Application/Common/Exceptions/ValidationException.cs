using FluentValidation.Results;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
    : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }

    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Format and retrieve all existing Errors.
    /// </summary>
    /// <returns>All errors as formated string.</returns>
    public string GetErrorsMessage()
    {
        string errorMessage = String.Empty;
        foreach (var error in Errors)
        {
            errorMessage = String.Join("\n", error.Value);
        }

        return errorMessage;
    }


}
