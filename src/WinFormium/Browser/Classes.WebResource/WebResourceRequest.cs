// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

using System.Collections.Specialized;
using System.Text.Encodings.Web;

namespace WinFormium.Browser;

/// <summary>
/// Represents a web resource request, including URL, headers, method, body, and parsed data.
/// </summary>
public sealed class WebResourceRequest
{
    /// <summary>
    /// Gets the request URI.
    /// </summary>
    public Uri Uri { get; }

    /// <summary>
    /// Gets the request URL without query string.
    /// </summary>
    public string RequestUrl
    {
        get
        {
            var original = Uri.OriginalString;
            if (original.Contains('?'))
            {
                return original.Substring(0, original.IndexOf("?"));
            }

            return original;
        }
    }

    /// <summary>
    /// Gets the request headers.
    /// </summary>
    public NameValueCollection? Headers { get; }

    /// <summary>
    /// Gets the uploaded file paths, if any.
    /// </summary>
    public string[] UploadFiles { get; }

    /// <summary>
    /// Gets the raw request body data.
    /// </summary>
    public byte[]? RawData { get; }

    private const string CONTENT_TYPE_FORM_URL_ENCODED = "application/x-www-form-urlencoded";
    private const string CONTENT_TYPE_APPLICATION_JSON = "application/json";
    private const string CONTENT_TYPE_MULTIPART_FORM_DATA = "multipart/form-data";
    private readonly string _method;

    /// <summary>
    /// Gets the raw CefRequest object.
    /// </summary>
    public CefRequest RawRequest { get; }

    /// <summary>
    /// Gets the relative path of the request URI.
    /// </summary>
    public string RelativePath => $"{Uri?.LocalPath ?? string.Empty}".TrimStart('/');

    /// <summary>
    /// Gets the file name from the relative path.
    /// </summary>
    public string FileName => Path.GetFileName(RelativePath);

    /// <summary>
    /// Gets the file extension from the file name.
    /// </summary>
    public string FileExtension => Path.GetExtension(FileName).TrimStart('.');

    /// <summary>
    /// Gets a value indicating whether the request has a file name.
    /// </summary>
    public bool HasFileName => !string.IsNullOrEmpty(FileName);

    /// <summary>
    /// Gets the parsed query string parameters.
    /// </summary>
    public NameValueCollection? QueryString { get; } = null;

    /// <summary>
    /// Gets the parsed form data, if available.
    /// </summary>
    public NameValueCollection? FormData { get; } = null;

    /// <summary>
    /// Gets the JSON data as a string, if available.
    /// </summary>
    public string? JsonData { get; } = null;

    /// <summary>
    /// Gets a value indicating whether the request content type is JSON.
    /// </summary>
    public bool IsJson
    {
        get
        {
            if (string.IsNullOrEmpty(ContentType))
            {
                return false;
            }

            return ContentType.Contains(CONTENT_TYPE_APPLICATION_JSON);
        }
    }

    /// <summary>
    /// Gets the encoding of the request content.
    /// </summary>
    public Encoding ContentEncoding
    {
        get
        {
            var encoding = ContentType;

            if (string.IsNullOrEmpty(encoding) || !encoding.Contains("charset="))
            {
                encoding = "utf-8";
            }
            else
            {
                // match "charset=xxx"
                var match = Regex.Match(encoding, @"(?<=charset=)(([^;,\r\n]))*");

                if (match.Success)
                {
                    encoding = match.Value;
                }
            }

            return Encoding.GetEncoding(encoding);
        }
    }

    /// <summary>
    /// Gets the request body as a string using the detected encoding.
    /// </summary>
    public string StringContent
    {
        get
        {
            if (RawData == null)
            {
                return string.Empty;
            }

            return ContentEncoding.GetString(RawData);
        }
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    /// <summary>
    /// Deserializes the JSON request body to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="jsonTypeInfo">The JSON type info for deserialization.</param>
    /// <returns>The deserialized object, or default if deserialization fails.</returns>
    public T? DeserializeObjectFromJson<T>(JsonTypeInfo<T> jsonTypeInfo)
    {
        if (IsJson && RawData != null)
        {
            try
            {
                return JsonSerializer.Deserialize(Encoding.UTF8.GetString(RawData), jsonTypeInfo);
            }
            catch
            {
                return default;
            }
        }

        return default;
    }

    /// <summary>
    /// Gets the HTTP method of the request.
    /// </summary>
    public WebResourceRequestMethod Method
    {
        get
        {
            if (Enum.TryParse(_method, out WebResourceRequestMethod value))
            {
                return value;
            }

            return WebResourceRequestMethod.All;
        }
    }

    /// <summary>
    /// Gets the content type of the request.
    /// </summary>
    public string ContentType => Headers?.Get("Content-Type") ?? string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebResourceRequest"/> class.
    /// </summary>
    /// <param name="uri">The request URI.</param>
    /// <param name="method">The HTTP method.</param>
    /// <param name="headers">The request headers.</param>
    /// <param name="postData">The raw post data.</param>
    /// <param name="uploadFiles">The uploaded file paths.</param>
    /// <param name="cefRequest">The raw CefRequest object.</param>
    internal WebResourceRequest(Uri uri, string method, NameValueCollection? headers, byte[] postData, string[] uploadFiles, CefRequest cefRequest)
    {
        Uri = uri;
        _method = method;
        Headers = headers;
        RawData = postData;
        UploadFiles = uploadFiles;
        RawRequest = cefRequest;
        QueryString = ProcessQueryString(uri.Query);

        if (ContentType != null && ContentType.Contains(CONTENT_TYPE_FORM_URL_ENCODED) && RawData != null)
        {
            FormData = ProcessFormUrlEncodedData(RawData);
        }
        else if (ContentType != null && ContentType.Contains(CONTENT_TYPE_MULTIPART_FORM_DATA) && RawData != null)
        {
            FormData = ProcessFormData(RawData);
        }
        else
        {
            FormData = new NameValueCollection();
        }

        if (IsJson && RawData != null)
        {
            try
            {
                JsonData = JsonSerializer.Serialize(Encoding.UTF8.GetString(RawData), WinFormiumJsonSerializerContext.Default.String);
            }
            catch
            {
                JsonData = string.Empty;
            }
        }
    }

    /// <summary>
    /// Processes multipart/form-data and extracts form fields.
    /// </summary>
    /// <param name="bytes">The raw form data bytes.</param>
    /// <returns>A collection of form fields.</returns>
    private NameValueCollection ProcessFormData(byte[] bytes)
    {
        var formData = ContentEncoding.GetString(bytes);

        var fields = new NameValueCollection();

        var boundary = GetBoundary(formData);

        formData = formData.Replace($"{boundary}--", null);

        var boundaryParts = formData.Split(new[] { boundary }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in boundaryParts)
        {
            var lines = part.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string? fieldName = null;
            string? fieldValue = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("Content-Disposition: form-data;"))
                {
                    var dispositionParts = line.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var partInfo in dispositionParts)
                    {
                        if (partInfo.StartsWith("name="))
                        {
                            fieldName = partInfo.Substring(5).Trim('"');
                        }
                    }
                }
                else if (line.StartsWith("Content-Type:"))
                {
                    // Handle file content type if needed
                }
                else
                {
                    fieldValue = line;
                }
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                fields.Add(fieldName, fieldValue);
            }
        }

        return fields;
    }

    /// <summary>
    /// Extracts the boundary string from multipart/form-data.
    /// </summary>
    /// <param name="formData">The form data as a string.</param>
    /// <returns>The boundary string.</returns>
    private string GetBoundary(string formData)
    {
        var endIndex = formData.IndexOf("\r\n", StringComparison.OrdinalIgnoreCase);
        return formData[..endIndex];
    }

    /// <summary>
    /// Processes application/x-www-form-urlencoded data and extracts form fields.
    /// </summary>
    /// <param name="rawData">The raw form data bytes.</param>
    /// <returns>A collection of form fields.</returns>
    private NameValueCollection ProcessFormUrlEncodedData(byte[] rawData)
    {
        var query = ContentEncoding.GetString(rawData);
        var retval = new NameValueCollection();
        query = query.Trim('?');

        foreach (var pair in query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var keyvalue = pair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (keyvalue.Length == 2)
            {
                retval.Add(keyvalue[0].ToLower(), Uri.UnescapeDataString(keyvalue[1]));
            }
            else if (keyvalue.Length == 1)
            {
                retval.Add(keyvalue[0].ToLower(), null);
            }
        }

        return retval;
    }

    /// <summary>
    /// Processes the query string and extracts parameters.
    /// </summary>
    /// <param name="query">The query string.</param>
    /// <returns>A collection of query parameters.</returns>
    private NameValueCollection ProcessQueryString(string query)
    {
        var retval = new NameValueCollection();

        query = query.Trim('?');
        foreach (var pair in query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var keyvalue = pair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (keyvalue.Length == 2)
            {
                retval.Add(keyvalue[0].ToLower(), Uri.UnescapeDataString(keyvalue[1]));
            }
            else if (keyvalue.Length == 1)
            {
                retval.Add(keyvalue[0].ToLower(), null);
            }
        }

        return retval;
    }
}
