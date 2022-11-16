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
///     An interface that exposes methods for delivering the contents of an 
///     instnace of the <see cref="MailMessage"/> class.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    ///     Synchronously delievers the given instance of the 
    ///     <see cref="MailMessage"/> class.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to deliver.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    SendResult Send(MailMessage email, CancellationToken token = default(CancellationToken));

    /// <summary>
    ///     Asynchronously delievers the given instance of the 
    ///     <see cref="MailMessage"/> class.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to deliver.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    Task<SendResult> SendAsync(MailMessage email, CancellationToken token = default(CancellationToken));
}