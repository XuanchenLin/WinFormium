// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.JavaScript;

/// <summary>
/// Represents a JavaScript Promise object for remote invocation and result handling.
/// </summary>
public class JavaScriptPromiseContext
{
    /// <summary>
    /// Stores the unique identifier for the remote call associated with this promise.
    /// </summary>
    private string? _remoteCallId = null;

    /// <summary>
    /// Generates and returns a unique call identifier for the remote promise invocation.
    /// </summary>
    /// <returns>
    /// The unique call identifier as a string.
    /// </returns>
    internal string GenerateCallId()
    {
        if (_remoteCallId is null)
        {
            _remoteCallId = Guid.NewGuid().ToString("N");
        }
        return _remoteCallId;
    }

    /// <summary>
    /// Gets the remote call identifier for this promise.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the promise function has not been initialized with a call identifier.
    /// </exception>
    internal string RemoteCallId => _remoteCallId is null ? throw new InvalidOperationException("This is not a promise function.") : _remoteCallId;

    /// <summary>
    /// Gets the object identifier associated with this promise.
    /// </summary>
    internal int ObjectId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JavaScriptPromiseContext"/> class with the specified object identifier.
    /// </summary>
    /// <param name="objectId">The object identifier for the promise.</param>
    internal JavaScriptPromiseContext(int objectId)
    {
        ObjectId = objectId;
    }

    /// <summary>
    /// Resolves the promise with an optional return value in JSON format.
    /// </summary>
    /// <param name="returnValueAsJson">The return value as a JSON string, or <c>null</c> if not applicable.</param>
    public void Resolve(string? returnValueAsJson = null)
    {
        CallRemoteResovle?.Invoke(returnValueAsJson);
    }

    /// <summary>
    /// Rejects the promise with an optional reason.
    /// </summary>
    /// <param name="reason">The reason for rejection, or <c>null</c> if not specified.</param>
    public void Reject(string? reason = null)
    {
        CallRemoteReject?.Invoke(reason);
    }

    /// <summary>
    /// Gets or sets the action to invoke when the promise is resolved.
    /// </summary>
    internal Action<string?>? CallRemoteResovle { get; set; }

    /// <summary>
    /// Gets or sets the action to invoke when the promise is rejected.
    /// </summary>
    internal Action<string?>? CallRemoteReject { get; set; }
}
