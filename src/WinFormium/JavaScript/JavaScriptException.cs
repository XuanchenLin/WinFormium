// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Represents an exception that occurs during JavaScript evaluation in the WinFormium environment.
/// </summary>
public class JavaScriptException : Exception
{
    /// <summary>
    /// Contains detailed information about the JavaScript evaluation exception.
    /// </summary>
    private readonly JavaScriptEvaluationException _details;

    /// <summary>
    /// Gets the zero-based column number at which the error starts.
    /// </summary>
    public int StartColumn => _details.StartColumn;

    /// <summary>
    /// Gets the zero-based character position at which the error starts.
    /// </summary>
    public int StartPosition => _details.StartPosition;

    /// <summary>
    /// Gets the zero-based column number at which the error ends.
    /// </summary>
    public int EndColumn => _details.EndColumn;

    /// <summary>
    /// Gets the zero-based character position at which the error ends.
    /// </summary>
    public int EndPosition => _details.EndPosition;

    /// <summary>
    /// Gets the one-based line number where the error occurred.
    /// </summary>
    public int LineNumber => _details.LineNumber;

    /// <summary>
    /// Gets the name of the script resource where the error occurred.
    /// </summary>
    public string ScriptResourceName => _details.ScriptResourceName;

    /// <summary>
    /// Gets the source line of code where the error occurred.
    /// </summary>
    public string SourceLine => _details.SourceLine;

    /// <summary>
    /// Initializes a new instance of the <see cref="JavaScriptException"/> class with a specified error message and details.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="details">The <see cref="JavaScriptEvaluationException"/> containing detailed error information.</param>
    internal JavaScriptException(string message, JavaScriptEvaluationException details) : base($"[JavaScript]{message}")
    {
        _details = details;
    }

}
