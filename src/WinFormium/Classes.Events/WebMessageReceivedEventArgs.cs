// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium;

/// <summary>
/// Represents the delegate for handling the event when a web message is received from the browser.
/// </summary>
/// <param name="args">The event data containing information about the received web message.</param>
public delegate void WebMessageReceivedDelegate(WebMessageReceivedEventArgs args);

/// <summary>
/// Provides data for the event that is raised when a web message is received from the browser.
/// </summary>
public class WebMessageReceivedEventArgs : EventArgs
{
    private readonly JavaScript.JavaScriptEngine.JsPostWebMessageToBrowserMessage _message;

    /// <summary>
    /// Gets the source of the web message.
    /// </summary>
    public string Source => _message.Source ?? string.Empty;

    /// <summary>
    /// Gets the browser identifier associated with the message.
    /// </summary>
    internal int BrowserId => _message.BrowserId;

    /// <summary>
    /// Gets the frame identifier associated with the message.
    /// </summary>
    internal long FrameId => _message.FrameId;

    /// <summary>
    /// Attempts to get the web message as a JSON string, or returns null if the data is empty or invalid.
    /// </summary>
    /// <returns>
    /// The web message as a JSON string if parsing is successful; otherwise, null.
    /// </returns>
    public string? TryGetWebMessageAsString()
    {
        var data = _message.Data;

        if (string.IsNullOrWhiteSpace(data)) return null;

        try
        {

            if ((data.StartsWith("'") && data.EndsWith("'")) || data.StartsWith("\"") && data.EndsWith("\""))
            {
                data = data[1..^1];


                data = Regex.Unescape(data);
            }
        }
        catch
        {

        }

        return data;
    }

    /// <summary>
    /// Gets the web message data as a JSON string, unescaping if necessary.
    /// </summary>
    public string WebMessageAsJson
    {
        get
        {
            if (_message.Data is null) return string.Empty;

            var data = _message.Data;

            //if ((data.StartsWith("'") && data.EndsWith("'")) || data.StartsWith("\"") && data.EndsWith("\""))
            //{
            //    data = data[1..^1];


            //    data = Regex.Unescape(data);
            //}

            return data;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebMessageReceivedEventArgs"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message received from the browser.</param>
    internal WebMessageReceivedEventArgs(JavaScript.JavaScriptEngine.JsPostWebMessageToBrowserMessage message)
    {
        _message = message;
    }
}
