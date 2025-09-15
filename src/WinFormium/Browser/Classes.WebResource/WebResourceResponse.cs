// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Collections.Specialized;

namespace WinFormium.Browser;
/// <summary>
/// Represents a response for a web resource, including content, headers, and status information.
/// </summary>
public sealed class WebResourceResponse : IDisposable
{
    /// <summary>
    /// Gets or sets the content body stream of the response.
    /// </summary>
    public Stream? ContentBody { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    public int HttpStatus { get; set; } = WebResourceStatusCodes.Status200OK;

    /// <summary>
    /// Gets the length of the content body stream.
    /// </summary>
    public long Length => ContentBody?.Length ?? 0;

    /// <summary>
    /// Gets or sets the content type of the response.
    /// </summary>
    public string? ContentType { get; set; } = "text/plain";

    /// <summary>
    /// Gets the collection of HTTP headers for the response.
    /// </summary>
    public NameValueCollection Headers { get; } = new NameValueCollection();

    /// <summary>
    /// Gets or sets the URL associated with the response.
    /// </summary>
    public string? Url { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebResourceResponse"/> class with optional content and content type.
    /// </summary>
    /// <param name="buff">The content as a byte array.</param>
    /// <param name="contentType">The MIME type of the content.</param>
    public WebResourceResponse(byte[]? buff = null, string? contentType = null)
    {
        if (!string.IsNullOrEmpty(contentType))
        {
            ContentType = contentType;
        }

        //Headers.Set("Content-Type", ContentType);

        if (buff != null)
        {
            ContentBody = new MemoryStream(buff);
        }

        HttpStatus = WebResourceStatusCodes.Status200OK;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ContentBody?.Close();
        ContentBody?.Dispose();
        ContentBody = null;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets the response content using a byte array and optional content type.
    /// </summary>
    /// <param name="buff">The content as a byte array.</param>
    /// <param name="contentType">The MIME type of the content.</param>
    internal void Content(byte[] buff, string? contentType = null)
    {
        if (!string.IsNullOrEmpty(contentType))
        {
            ContentType = contentType;
        }

        Headers.Set("Content-Type", ContentType);

        if (ContentBody != null)
        {
            ContentBody.Dispose();
            ContentBody = null;
        }

        ContentBody = new MemoryStream(buff);

        HttpStatus = WebResourceStatusCodes.Status200OK;
    }

    /// <summary>
    /// Sets the response content as JSON using the specified object and optional serializer options.
    /// </summary>
    /// <param name="data">The object to serialize as JSON.</param>
    /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
    internal void JsonContent(object data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, jsonSerializerOptions));

        Content(bytes, "application/json");
    }

    /// <summary>
    /// Sets the response content as JSON using the specified generic object and optional serializer options.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize as JSON.</param>
    /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
    internal void JsonContent<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, jsonSerializerOptions));

        Content(bytes, "application/json");
    }

    /// <summary>
    /// Sets the response content as plain text using UTF-8 encoding.
    /// </summary>
    /// <param name="text">The text content.</param>
    internal void TextContent(string text)
    {
        TextContent(text, Encoding.UTF8);
    }

    /// <summary>
    /// Sets the response content as plain text using the specified encoding.
    /// </summary>
    /// <param name="text">The text content.</param>
    /// <param name="encoding">The encoding to use.</param>
    internal void TextContent(string text, Encoding encoding)
    {
        Content(text, "text/plain", encoding);
    }

    /// <summary>
    /// Sets the response content as plain text using UTF-8 encoding.
    /// </summary>
    /// <param name="content">The text content.</param>
    internal void Content(string content)
    {
        Content(Encoding.UTF8.GetBytes(content), "text/plain");
    }

    /// <summary>
    /// Sets the response content as the specified content type using UTF-8 encoding.
    /// </summary>
    /// <param name="content">The content as a string.</param>
    /// <param name="contentType">The MIME type of the content.</param>
    internal void Content(string content, string contentType)
    {
        Content(Encoding.UTF8.GetBytes(content), contentType);
    }

    /// <summary>
    /// Sets the response content as the specified content type and encoding.
    /// </summary>
    /// <param name="content">The content as a string.</param>
    /// <param name="contentType">The MIME type of the content.</param>
    /// <param name="encoding">The encoding to use.</param>
    internal void Content(string content, string contentType, Encoding encoding)
    {
        Content(encoding.GetBytes(content), contentType);
    }
}
