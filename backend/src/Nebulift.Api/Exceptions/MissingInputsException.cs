namespace Nebulift.Api.Exceptions;

/// <summary>
/// Throws when a required input is missing.
/// </summary>
public class MissingInputsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingInputsException"/> class.
    /// </summary>
    public MissingInputsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingInputsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public MissingInputsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingInputsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public MissingInputsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}