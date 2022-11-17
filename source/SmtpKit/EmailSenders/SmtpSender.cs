/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace SmtpKit.EmailSenders;

/// <summary>
///     An implementation of <see cref="IEmailSender"/> that sends a given
///     <see cref="MailMessage"/> instance to an SMTP server for deliver, using
///     an instance of the <see cref="SmtpClient"/> class.
/// </summary>
public sealed class SmtpSender : IEmailSender
{
    /// <summary>
    ///     A factory used to create instances of the <see cref="SmtpClient"/>
    ///     class.
    /// </summary>
    private class SmtpClientFactory
    {
        private readonly string _host;
        private readonly int _port;
        private readonly NetworkCredential? _credentials;
        private readonly X509Certificate? _certificate;

        /// <summary>
        ///     Initializes a new instance of the 
        ///     <see cref="SmtpClientFactory"/> class.
        /// </summary>
        /// <param name="host">
        ///     A <see cref="string"/> containing the IPv4 address or host name
        ///     of the Smtp server to connect to.
        /// </param>
        /// <param name="port">
        ///     An <see cref="int"/> value that represents the port to connect 
        ///     to on the <paramref name="host"/>.
        /// </param>
        public SmtpClientFactory(string host, int port) => (_host, _port) = (host, port);

        /// <summary>
        ///     Initializes a new instance of the 
        ///     <see cref="SmtpClientFactory"/> class.
        /// </summary>
        /// <param name="host">
        ///     A <see cref="string"/> containing the IPv4 address or host name
        ///     of the Smtp server to connect to.
        /// </param>
        /// <param name="port">
        ///     An <see cref="int"/> value that represents the port to connect to
        ///     on the <paramref name="host"/>.
        /// </param>
        /// <param name="username">
        ///     A <see cref="string"/> containing the username used to authenticate
        ///     with the Smtp server.
        /// </param>
        /// <param name="password">
        ///     A <see cref="string"/> containing the password used to authenticate
        ///     with the Smtp server.
        /// </param>
        public SmtpClientFactory(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _credentials = new NetworkCredential(username, password);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SmtpSender"/> class.
        /// </summary>
        /// <param name="host">
        ///     A <see cref="string"/> containing the IPv4 address or host name
        ///     of the Smtp server to connect to.
        /// </param>
        /// <param name="port">
        ///     An <see cref="int"/> value that represents the port to connect to
        ///     on the <paramref name="host"/>.
        /// </param>
        /// <param name="certificate">
        ///     An <see cref="X509Certificate"/> certificate used to establish
        ///     a Secure Sockets Layer (SSL) connection when connecting to the
        ///     Smtp server.  
        /// </param>
        public SmtpClientFactory(string host, int port, X509Certificate certificate) =>
            (_host, _port, _certificate) = (host, port, certificate);

        /// <summary>
        ///     Creates a new instance of the <see cref="SmtpClient"/> class
        ///     using the host and port this <see cref="SmtpClientFactory"/>
        ///     was initialized with.
        /// </summary>
        /// <returns>
        ///     A new instance of the <see cref="SmtpClient"/> class.
        /// </returns>
        public SmtpClient Create()
        {
            SmtpClient client = new(_host, _port);

            if (_certificate is not null)
            {
                client.ClientCertificates.Add(_certificate);
            }

            if (_credentials is not null)
            {
                client.Credentials = _credentials;
            }

            return client;
        }
    }

    private readonly SmtpClientFactory _factory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SmtpSender"/> class.
    /// </summary>
    /// <param name="host">
    ///     A <see cref="string"/> containing the IPv4 address or host name
    ///     of the Smtp server to connect to.
    /// </param>
    /// <param name="port">
    ///     An <see cref="int"/> value that represents the port to connect to
    ///     on the <paramref name="host"/>.
    /// </param>
    public SmtpSender(string host, int port) => _factory = new(host, port);

    /// <summary>
    ///     Initializes a new instance of the <see cref="SmtpSender"/> class.
    /// </summary>
    /// <param name="host">
    ///     A <see cref="string"/> containing the IPv4 address or host name
    ///     of the Smtp server to connect to.
    /// </param>
    /// <param name="port">
    ///     An <see cref="int"/> value that represents the port to connect to
    ///     on the <paramref name="host"/>.
    /// </param>
    /// <param name="username">
    ///     A <see cref="string"/> containing the username used to authenticate
    ///     with the Smtp server.
    /// </param>
    /// <param name="password">
    ///     A <see cref="string"/> containing the password used to authenticate
    ///     with the Smtp server.
    /// </param>
    public SmtpSender(string host, int port, string username, string password) =>
        _factory = new(host, port, username, password);

    /// <summary>
    ///     Initializes a new instance of the <see cref="SmtpSender"/> class.
    /// </summary>
    /// <param name="host">
    ///     A <see cref="string"/> containing the IPv4 address or host name
    ///     of the Smtp server to connect to.
    /// </param>
    /// <param name="port">
    ///     An <see cref="int"/> value that represents the port to connect to
    ///     on the <paramref name="host"/>.
    /// </param>
    /// <param name="certificate">
    ///     An <see cref="X509Certificate"/> certificate used to establish
    ///     a Secure Sockets Layer (SSL) connection when connecting to the
    ///     Smtp server.  
    /// </param>
    public SmtpSender(string host, int port, X509Certificate certificate) =>
        _factory = new(host, port, certificate);

    /// <summary>
    ///     Synchronously sends the specified <see cref="MailMessage"/> instance
    ///     to an SMTP server for delivery.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to send to the SMTP server
    ///     for delivery.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    public SendResult Send(MailMessage email, CancellationToken token = default) =>
        Task.Run(() => SendAsync(email, token)).Result;


    /// <summary>
    ///     Asynchronously sends the specified <see cref="MailMessage"/>
    ///     instance to an SMTP server for delivery.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to send to the SMTP server
    ///     for delivery.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    public async Task<SendResult> SendAsync(MailMessage email, CancellationToken token = default)
    {
        SendResult result = new();

        if (token.IsCancellationRequested)
        {
            result.Errors.Add("Message was cancelled by token before sending");
        }

        using (SmtpClient client = _factory.Create())
        {
            await client.SendMailAsyncNoDeadlock(email, token);
        }

        return result;
    }
}