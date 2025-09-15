// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using static WinFormium.JavaScript.JavaScriptEngine;

namespace WinFormium.JavaScript;
/// <summary>
/// Represents the result of executing a JavaScript script in the WinFormium environment.
/// Provides access to the execution status, result data, and any exception information.
/// </summary>
public sealed class JavaScriptExecuteScriptResult
{
    /// <summary>
    /// The underlying message containing the evaluation result details.
    /// </summary>
    private readonly JsEvaluationCompleteMessage _args;

    /// <summary>
    /// Initializes a new instance of the <see cref="JavaScriptExecuteScriptResult"/> class
    /// with the specified evaluation completion message.
    /// </summary>
    /// <param name="args">The message containing the result of the JavaScript evaluation.</param>
    internal JavaScriptExecuteScriptResult(JsEvaluationCompleteMessage args)
    {
        _args = args;
    }

    /// <summary>
    /// Gets a value indicating whether the script execution succeeded.
    /// </summary>
    public bool Succeeded => _args.Success;

    /// <summary>
    /// Gets the exception information if the script execution failed; otherwise, <c>null</c>.
    /// </summary>
    public JavaScriptEvaluationException? Exception => _args.JsException != null ? JavaScriptEvaluationException.FromJson(_args.JsException) : null;

    /// <summary>
    /// Gets the result of the script execution as a JSON string, or <c>null</c> if no result is available.
    /// Handles unescaping and removal of surrounding quotes if present.
    /// </summary>
    public string? ResultAsJson
    {
        get
        {
            if (_args.Data == null)
                return null;

            var data = _args.Data;

            if (data.StartsWith("'") && data.EndsWith("'") || data.StartsWith("\"") && data.EndsWith("\""))
            {
                data = data[1..^1];


                data = Regex.Unescape(data);
            }

            return data;

        }
    }

    /// <summary>
    /// Attempts to parse the result data as a JSON web message string.
    /// </summary>
    /// <param name="result">When this method returns, contains the web message as a string if parsing succeeded; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the data was successfully parsed as a JSON web message; otherwise, <c>false</c>.</returns>
    public bool TryGetWebMessageAsString(out string? result)
    {
        var data = _args.Data;

        result = null;
        if (string.IsNullOrWhiteSpace(data)) return false;

        try
        {
            var jdoc = JsonDocument.Parse(data);

            result = jdoc.RootElement.GetRawText();

            return true;
        }
        catch
        {

        }

        return false;
    }


}
