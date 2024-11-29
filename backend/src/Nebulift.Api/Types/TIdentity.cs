namespace Nebulift.Api.Types;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the identity of a template, including its name and associated technologies.
/// </summary>
public struct TIdentity : IEquatable<TIdentity>
{
    /// <summary>
    /// Gets or sets the name of the identity.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of technologies associated with the identity.
    /// </summary>
    public List<string> Technologies { get; set; }

    /// <summary>
    /// Determines whether two <see cref="TIdentity"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TIdentity"/> instance.</param>
    /// <param name="right">The second <see cref="TIdentity"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TIdentity left, TIdentity right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="TIdentity"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TIdentity"/> instance.</param>
    /// <param name="right">The second <see cref="TIdentity"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TIdentity left, TIdentity right) => !left.Equals(right);

    /// <summary>
    /// Determines whether the current <see cref="TIdentity"/> is equal to another <see cref="TIdentity"/>.
    /// </summary>
    /// <param name="other">The other <see cref="TIdentity"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TIdentity other)
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
    /// Determines whether the specified object is equal to the current <see cref="TIdentity"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="TIdentity"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="TIdentity"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is TIdentity other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="TIdentity"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TIdentity"/>.</returns>
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
}