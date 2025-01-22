namespace Nebulift.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when a bad request is made.
    /// </summary>
    public class FailedTemplateExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedTemplateExecutionException"/> class.
        /// </summary>
        public FailedTemplateExecutionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedTemplateExecutionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FailedTemplateExecutionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedTemplateExecutionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public FailedTemplateExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}