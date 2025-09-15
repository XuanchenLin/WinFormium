// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser.ProcessCommunication.NamedPipe;
/// <summary>
/// Provides a wrapper for stream-based inter-process communication using named pipes with Unicode encoding.
/// </summary>
class PipeStream : IDisposable
{
    /// <summary>
    /// The underlying stream used for communication.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// The encoding used for reading and writing messages.
    /// </summary>
    private readonly UnicodeEncoding _streamEncoding;

    /// <summary>
    /// Initializes a new instance of the <see cref="PipeStream"/> class with the specified stream.
    /// </summary>
    /// <param name="stream">The stream to use for communication.</param>
    public PipeStream(Stream stream)
    {
        _stream = stream;
        _streamEncoding = new UnicodeEncoding();
    }

    /// <summary>
    /// Writes a string message to the stream.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void Write(string message)
    {
        using var writer = new BinaryWriter(_stream, _streamEncoding, true);
        var messageBytes = _streamEncoding.GetBytes(message);
        var length = Convert.ToInt32(messageBytes.Length);
        writer.Write(length);
        writer.Write(messageBytes);
    }

    /// <summary>
    /// Reads a string message from the stream.
    /// </summary>
    /// <returns>The message read from the stream, or an empty string if the stream cannot be read.</returns>
    public string Read()
    {
        if (!_stream.CanRead) return string.Empty;

        using var reader = new BinaryReader(_stream, _streamEncoding, true);
        var length = reader.ReadInt32();
        var message = reader.ReadBytes(length);
        return _streamEncoding.GetString(message);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _stream?.Dispose();
    }
}
