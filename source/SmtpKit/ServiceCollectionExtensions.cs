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
using Microsoft.Extensions.DependencyInjection;

namespace SmtpKit;

/// <summary>
///     Contains extension method for <see cref="IServiceCollection"/> used to
///     add <see cref="SmtpKit"/> to the services container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a new <see cref="IEmailFactory"/> instance to the
    ///     <see cref="IServiceCollection"/> instance as a scoped services.
    /// </summary>
    /// <remarks>
    ///     When this method returns, you will be provided with a new
    ///     <see cref="IEmailFactoryBuilder"/> instance.  You must use one of
    ///     the additional methods provided by this instance to finish
    ///     coniguring the services.
    /// </remarks>
    /// <param name="services">
    ///     The <see cref="IServiceCollection"/> instance to add the 
    ///     <see cref="IEmailFactory"/> instance too.
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
    /// <returns>
    ///     A new <see cref="IEmailFactoryBuilder"/> instance to be used to
    ///     finalize configuration of the service.
    /// </returns>
    public static IEmailFactoryBuilder AddSmtpKit(this IServiceCollection services, string from, string? name)
    {
        return new EmailFactoryBuilder(services, from, name);
    }
}