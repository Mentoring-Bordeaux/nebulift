namespace Nebulift.Api.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the identity of a template, including its name and associated technologies.
/// </summary>
public struct Identity : IEquatable<Identity>
{
    /// <summary>
    /// Gets or sets the name of the identity.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of technologies associated with the identity.
    /// </summary>
    [JsonPropertyName("technologies")]
    public List<string> Technologies { get; set; }

    /// <summary>
    /// Determines whether two <see cref="Identity"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Identity"/> instance.</param>
    /// <param name="right">The second <see cref="Identity"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Identity left, Identity right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Identity"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Identity"/> instance.</param>
    /// <param name="right">The second <see cref="Identity"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Identity left, Identity right) => !left.Equals(right);

    /// <summary>
    /// Determines whether the current <see cref="Identity"/> is equal to another <see cref="Identity"/>.
    /// </summary>
    /// <param name="other">The other <see cref="Identity"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(Identity other)
    {
        // Compare the Name properties
        bool nameEquals = string.Equals(Name, other.Name, StringComparison.Ordinal);

        // Compare the Technologies lists
        bool technologiesEquals = Technologies != null && other.Technologies != null
            ? Technologies.Count == other.Technologies.Count &&
              !Technologies.Except(other.Technologies).Any()
            : Technologies == other.Technologies;

        return nameEquals && technologiesEquals;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Identity"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Identity"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Identity"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is Identity other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="Identity"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Identity"/>.</returns>
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
    /// Returns a string representation of the current <see cref="Identity"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="Identity"/>.</returns>
    public override string ToString()
    {
        var technologies = Technologies != null && Technologies.Any()
            ? string.Join(", ", Technologies)
            : string.Empty;

        return $"Name: {Name}, Technologies: [{technologies}]";
    }
}