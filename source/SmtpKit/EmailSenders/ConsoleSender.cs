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
using System.Net.Mail;

namespace SmtpKit.EmailSenders;

/// <summary>
///     An implementation of <see cref="IEmailSender"/> that outputs the 
///     contents of a <see cref="MailMessage"/> to the console window.
/// </summary>
internal sealed class ConsoleSender : IEmailSender
{
    /// <summary>
    ///     Creates a new instance of the <see cref="ConsoleSender"/> class.
    /// </summary>
    public ConsoleSender() { }

    /// <summary>
    ///     Synchronously outputs the contents of the given
    ///     <see cref="MailMessage"/> instance the console window.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to output to the console.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    public SendResult Send(MailMessage email, CancellationToken token = default(CancellationToken)) =>
        SendAsync(email, token).GetAwaiter().GetResult();

    /// <summary>
    ///     Asynchronously outputs the contents of the given
    ///     <see cref="MailMessage"/> instance to the console window.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to output to the console.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     A new <see cref="Task{TResult}"/> object containing the the result
    ///     of this asynchronous operation.
    /// </returns>
    public async Task<SendResult> SendAsync(MailMessage email, CancellationToken token = default(CancellationToken))
    {
        await Console.Out.WriteLineAsync(email.ToStringExtended());
        return new SendResult();
    }
}