namespace Nebulift.Api.Services.Blob;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the reference to a template source code.
/// </summary>
public readonly struct TemplateCodeReference : IEquatable<TemplateCodeReference>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateCodeReference"/> struct.
    /// </summary>
    /// <param name="url">The GitHub URL of the repository containing the template.</param>
    /// <param name="path">The path to the template within the repository (should contain the Pulumi code).</param>
    /// <param name="branch">The branch of the repository to use for the template.</param>
    [JsonConstructor]
    public TemplateCodeReference(Uri url, string path, string branch)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Branch = branch ?? throw new ArgumentNullException(nameof(branch));
    }

    /// <summary>
    /// Gets the GitHub URL of the repository containing the template.
    /// </summary>
    [JsonPropertyName("url")]
    public Uri Url { get; }

    /// <summary>
    /// Gets the path to the template within the repository (should contain the Pulumi code).
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; }

    /// <summary>
    /// Gets the branch of the repository to use for the template.
    /// </summary>
    [JsonPropertyName("branch")]
    public string Branch { get; }

    /// <summary>
    /// Determines whether two <see cref="TemplateCodeReference"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateCodeReference"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateCodeReference"/> instance.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TemplateCodeReference left, TemplateCodeReference right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="TemplateCodeReference"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TemplateCodeReference"/> instance.</param>
    /// <param name="right">The second <see cref="TemplateCodeReference"/> instance.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TemplateCodeReference left, TemplateCodeReference right) => !left.Equals(right);

    /// <summary>
    /// Determines whether the current <see cref="TemplateCodeReference"/> is equal to another <see cref="TemplateCodeReference"/>.
    /// </summary>
    /// <param name="other">The other <see cref="TemplateCodeReference"/> to compare to.</param>
    /// <returns><c>true</c> if the current instance is equal to the <paramref name="other"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TemplateCodeReference other)
    {
        // Compare the Url properties
        bool urlEquals = Url.Equals(other.Url);

        // Compare the Path properties
        bool pathEquals = string.Equals(Path, other.Path, StringComparison.Ordinal);

        // Compare the Branch properties
        bool branchEquals = string.Equals(Branch, other.Branch, StringComparison.Ordinal);

        return urlEquals && pathEquals && branchEquals;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="TemplateCodeReference"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="TemplateCodeReference"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="TemplateCodeReference"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is TemplateCodeReference other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="TemplateCodeReference"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TemplateCodeReference"/>.</returns>
    public override int GetHashCode() => HashCode.Combine(Url, Path, Branch);

    /// <summary>
    /// Returns a string representation of the current <see cref="TemplateCodeReference"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="TemplateCodeReference"/>.</returns>
    public override string ToString() => $"URL: {Url}, Path: {Path}, Branch: {Branch}";
}