// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Represents an exception that occurs during JavaScript evaluation.
/// </summary>
public sealed class JavaScriptEvaluationException
{
    /// <summary>
    /// Gets or sets the error message describing the exception.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the zero-based column number where the error starts.
    /// </summary>
    public required int StartColumn { get; set; }

    /// <summary>
    /// Gets or sets the zero-based character position where the error starts.
    /// </summary>
    public required int StartPosition { get; set; }

    /// <summary>
    /// Gets or sets the zero-based column number where the error ends.
    /// </summary>
    public required int EndColumn { get; set; }

    /// <summary>
    /// Gets or sets the zero-based character position where the error ends.
    /// </summary>
    public required int EndPosition { get; set; }

    /// <summary>
    /// Gets or sets the one-based line number where the error occurred.
    /// </summary>
    public required int LineNumber { get; set; }

    /// <summary>
    /// Gets or sets the name of the script resource where the error occurred.
    /// </summary>
    public required string ScriptResourceName { get; set; }

    /// <summary>
    /// Gets or sets the source line of code where the error occurred.
    /// </summary>
    public required string SourceLine { get; set; }

    /// <summary>
    /// Serializes this exception instance to a JSON string.
    /// </summary>
    /// <returns>A JSON string representation of this exception.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, WinFormiumJsonSerializerContext.Default.JavaScriptEvaluationException);
    }

    /// <summary>
    /// Deserializes a JSON string to a <see cref="JavaScriptEvaluationException"/> instance.
    /// </summary>
    /// <param name="json">The JSON string representing the exception.</param>
    /// <returns>A <see cref="JavaScriptEvaluationException"/> instance.</returns>
    public static JavaScriptEvaluationException FromJson(string json)
    {
        return JsonSerializer.Deserialize<JavaScriptEvaluationException>(json, WinFormiumJsonSerializerContext.Default.JavaScriptEvaluationException)!;
    }
}
