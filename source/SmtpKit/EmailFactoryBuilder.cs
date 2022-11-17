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

internal class EmailFactoryBuilder : IEmailFactoryBuilder
{
    private readonly string _from;
    private readonly string? _name;
    public IServiceCollection Services { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmailFactoryBuilder"/>
    ///     class.
    /// </summary>
    /// <param name="services">
    ///     The <see cref="IServiceCollection"/> instance that will be used to
    ///     add the <see cref="IEmailFactory"/> instance to.
    /// </param>
    /// <param name="from">
    ///     A <see cref="string"/> that contains the email address to set to 
    ///     the From field of all <see cref="IEmail"/> instances created by the
    ///     <see cref="IEmailFactory"/> instance being added to the services.
    /// </param>
    /// <param name="name">
    ///     An optional <see cref="string"/> that contains the display name to 
    ///     set for the <paramref name="from"/> email address.
    /// </param>
    internal EmailFactoryBuilder(IServiceCollection services, string from, string? name)
    {
        Services = services;
        _from = from;
        _name = name;
    }

    /// <summary>
    ///     Configures the <see cref="IEmailFactory"/> instance being added to
    ///     the services container to output the contents of the email messages
    ///     created by the factory to the console window.
    /// </summary>
    public void UseConsole() => AddEmailFactory(new ConsoleSender());

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
    public void UseSmtp(string host, int port) => AddEmailFactory(new SmtpSender(host, port));

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
    public void UseSmtp(string host, int port, string username, string password) =>
        AddEmailFactory(new SmtpSender(host, port, username, password));

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
    public void UseSmtp(string host, int port, X509Certificate certificate) =>
        AddEmailFactory(new SmtpSender(host, port, certificate));

    /// <summary>
    ///     Adds a new instance of <see cref="IEmailFactory"/> to the services
    ///     container.
    /// </summary>
    /// <param name="sender">
    ///     The instance of <see cref="IEmailSender"/> to be used by each
    ///     instance of <see cref="IEmail"/> creating by the
    ///     <see cref="IEmailFactory"/> instance being added.
    /// </param>
    private void AddEmailFactory(IEmailSender sender)
    {
        Services.AddScoped<IEmailFactory>((_) => new EmailFactory(new(_from, _name), sender));
    }
}