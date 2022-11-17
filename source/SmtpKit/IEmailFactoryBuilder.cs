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
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.DependencyInjection;

using SmtpKit.EmailSenders;

namespace SmtpKit;

public interface IEmailFactoryBuilder
{
    IServiceCollection Services { get; }

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to output the contents of the email messages
    ///     created by the factory to the console window.
    /// </summary>
    void UseConsole();

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to send the email messages created by the
    ///     factory to an SMTP server.
    /// </summary>
    /// <param name="host">
    ///     A <see cref="string"/> containing the IPv4 address or host name
    ///     of the Smtp server to connect to.
    /// </param>
    /// <param name="port">
    ///     An <see cref="int"/> value that represents the port to connect to
    ///     on the <paramref name="host"/>.
    /// </param>
    void UseSmtp(string host, int port);

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to send the email messages created by the
    ///     factory to an SMTP server.
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
    void UseSmtp(string host, int port, string username, string password);

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to send the email messages created by the
    ///     factory to an SMTP server.
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
    void UseSmtp(string host, int port, X509Certificate certificate);

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to send the email messages created by the
    ///     factory to a text file in the.
    /// </summary>
    /// <param name="outDir">
    ///     A <see cref="string"/> containing the absolute path to the directory
    ///     to output the email messages in.  If the directory does not exist,
    ///     it will be created.
    /// </param>
    void UseFileSystem(string outDir);
}