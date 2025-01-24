namespace Nebulift.Api.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the identity of a template, including its name and associated technologies.
/// </summary>
public struct TemplateIdentity : IEquatable<TemplateIdentity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateIdentity"/> struct.
    /// </summary>
    /// <param name="name">The name of the template identity.</param>
    /// <param name="technologies">The list of technologies associated with the identity.</param>
    [JsonConstructor]
    public TemplateIdentity(string name, IEnumerable<string> technologies,string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Technologies = technologies ?? throw new ArgumentNullException(nameof(technologies));
        Description = description;
    }
    

    /// <summary>
    /// Gets the name of the identity.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Gets the list of technologies associated with the identity.
    /// </summary>
    [JsonPropertyName("technologies")]
    public IEnumerable<string> Technologies { get; }

     /// <summary>
    /// Gets the description of the identity.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Determines whether two <see cref="TemplateIdentity"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateIdentity"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateIdentity"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TemplateIdentity left, TemplateIdentity right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="TemplateIdentity"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateIdentity"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateIdentity"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TemplateIdentity left, TemplateIdentity right) => !left.Equals(right);

    /// <summary>
    /// Determines whether the current <see cref="TemplateIdentity"/> is equal to another <see cref="TemplateIdentity"/>.
    /// </summary>
    /// <param name="other">The other <see cref="TemplateIdentity"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TemplateIdentity other)
    {
        // Compare the Name properties
        bool nameEquals = string.Equals(Name, other.Name, StringComparison.Ordinal);

        // Compare the Technologies lists
        bool technologiesEquals = Enumerable.SequenceEqual(Technologies, other.Technologies);

        return nameEquals && technologiesEquals;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="TemplateIdentity"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="TemplateIdentity"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="TemplateIdentity"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is TemplateIdentity other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="TemplateIdentity"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TemplateIdentity"/>.</returns>
    public override int GetHashCode()
    {
        // Use StringComparison.Ordinal for clarity and better performance
        int hash = Name?.GetHashCode(StringComparison.Ordinal) ?? 0;

        if (Technologies != null)
        {
            foreach (var tech in Technologies)
            {
                hash = HashCode.Combine(hash, tech.GetHashCode(StringComparison.Ordinal));
            }
        }

        return hash;
    }

    /// <summary>
    /// Returns a string representation of the current <see cref="TemplateIdentity"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="TemplateIdentity"/>.</returns>
    public override string ToString()
    {
        var technologies = Technologies != null && Technologies.Any()
            ? string.Join(", ", Technologies)
            : string.Empty;

        return $"Name: {Name}, Technologies: [{technologies}]";
    }
}