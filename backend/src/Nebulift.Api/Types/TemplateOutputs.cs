namespace Nebulift.Api.Types;

using System.Text.Json;

/// <summary>
/// Represents the outputs of a template.
/// </summary>
public readonly struct TemplateOutputs : IEquatable<TemplateOutputs>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateOutputs"/> struct.
    /// </summary>
    /// <param name="dictionary">The content of the template inputs.</param>
    public TemplateOutputs(Dictionary<string, string> dictionary)
    {
        Content = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    /// <summary>
    /// Gets the content of the template inputs.
    /// </summary>
    public Dictionary<string, string> Content { get; }

    /// <summary>
    /// Determines whether two <see cref="TemplateOutputs"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateOutputs"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateOutputs"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TemplateOutputs left, TemplateOutputs right) => Equals(left, right);

    /// <summary>
    /// Determines whether two <see cref="TemplateOutputs"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateOutputs"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateOutputs"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TemplateOutputs left, TemplateOutputs right) => !Equals(left, right);

    /// <summary>
    /// Determines whether the current <see cref="TemplateOutputs"/> is equal to another <see cref="TemplateOutputs"/>.
    /// </summary>
    /// <param name="other">The other <see cref="TemplateOutputs"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TemplateOutputs other) => Content.Equals(other.Content);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="TemplateOutputs"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="TemplateOutputs"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="TemplateOutputs"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is TemplateOutputs other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="TemplateOutputs"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TemplateOutputs"/>.</returns>
    public override int GetHashCode() => Content.GetHashCode();

    /// <summary>
    /// Returns a string representation of the current <see cref="TemplateOutputs"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="TemplateOutputs"/>.</returns>
    public override string ToString() => JsonSerializer.Serialize(Content);
}