namespace Nebulift.Api.Types;

using System.Text.Json.Nodes;

/// <summary>
/// Represents the inputs of a template.
/// </summary>
public struct TemplateInputs : IEquatable<TemplateInputs>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateInputs"/> struct.
    /// </summary>
    /// <param name="content">The content of the template inputs.</param>
    public TemplateInputs(JsonObject content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    /// <summary>
    /// Gets the content of the template inputs.
    /// </summary>
    public JsonObject Content { get; }

    /// <summary>
    /// Determines whether two <see cref="TemplateInputs"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateInputs"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateInputs"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TemplateInputs left, TemplateInputs right) => Equals(left, right);

    /// <summary>
    /// Determines whether two <see cref="TemplateInputs"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateInputs"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateInputs"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TemplateInputs left, TemplateInputs right) => !Equals(left, right);

    /// <summary>
    /// Determines whether the current <see cref="TemplateInputs"/> is equal to another <see cref="TemplateInputs"/>.
    /// </summary>
    /// <param name="other">The other <see cref="TemplateInputs"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TemplateInputs other) => Content.Equals(other.Content);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="TemplateInputs"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="TemplateInputs"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="TemplateInputs"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is TemplateInputs other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="TemplateInputs"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TemplateInputs"/>.</returns>
    public override int GetHashCode() => Content.GetHashCode();

    /// <summary>
    /// Returns a string representation of the current <see cref="TemplateInputs"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="TemplateInputs"/>.</returns>
    public override string ToString() => $"Content: {Content}";
}