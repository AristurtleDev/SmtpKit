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

using SmtpKit.EmailSenders;

namespace SmtpKit;

public interface IEmail
{
    /// <summary>
    ///     Sets the From field of the current <see cref="IEmail"/> instance to
    ///     the email address given.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to add.
    /// </param>
    /// <param name="displayName">
    ///     An optional <see cref="string"/> containing the display name to use
    ///     for the email address.  If <see langword="null"/> is provided, then
    ///     the <paramref name="emailAddress"/> value will be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail From(string emailAddress, string? displayName = default);

    /// <summary>
    ///     Sets the From field of the current <see cref="IEmail"/> instance
    ///     to the <see cref="MailAddress"/> instance given.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to set as the From field
    ///     of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail From(MailAddress address);

    /// <summary>
    ///     Adds a new email address to the To field of the current 
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to add.
    /// </param>
    /// <param name="displayName">
    ///     An optional <see cref="string"/> containing the display name to use
    ///     for the email address.  If <see langword="null"/> is provided, then
    ///     the <paramref name="emailAddress"/> value will be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail To(string emailAddress, string? displayName = default);

    /// <summary>
    ///     Adds a <see cref="MailAddress"/> instance to the To field of the 
    ///     current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to add to the To field of the
    ///     current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail To(MailAddress address);

    /// <summary>
    ///     Adds the elements of an <see langword="IEnumerable"/> to the To 
    ///     field of the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="addresses">
    ///     The <see langword="IEnumerable"> who's elements should be added to
    ///     the To field of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail To(IEnumerable<MailAddress> addresses);

    /// <summary>
    ///     Adds a new email address to the Carbon Copy (CC) field of the
    ///     current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to add.
    /// </param>
    /// <param name="displayName">
    ///     An optional <see cref="string"/> containing the display name to use
    ///     for the email address.  If <see langword="null"/> is provided, then
    ///     the <paramref name="emailAddress"/> value will be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Cc(string emailAddress, string? displayName = default);

    /// <summary>
    ///     Adds a <see cref="MailAddress"/> instance to the Carbon Copy (CC)
    ///     field of the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to add to the Carbon Copy
    ///     (CC) field of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Cc(MailAddress address);

    /// <summary>
    ///     Adds the elements of an <see langword="IEnumerable"/> to the Carbon
    ///     Copy (CC) field of the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="addresses">
    ///     The <see langword="IEnumerable"> who's elements should be added to
    ///     the Carbon Copy (BCC) field of the current <see cref="IEmail"/>
    ///     instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Cc(IEnumerable<MailAddress> addresses);

    /// <summary>
    ///     Adds a new email address to the Blind Carbon Copy (BCC) field of the
    ///     current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to add.
    /// </param>
    /// <param name="displayName">
    ///     An optional <see cref="string"/> containing the display name to use
    ///     for the email address.  If <see langword="null"/> is provided, then
    ///     the <paramref name="emailAddress"/> value will be used.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Bcc(string emailAddress, string? displayName = default);

    /// <summary>
    ///     Adds a <see cref="MailAddress"/> instance to the Blind Carbon Copy
    ///     (BCC) field of the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to add to the Blind Carbon
    ///     Copy (BCC) field of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Bcc(MailAddress address);

    /// <summary>
    ///     Adds the elements of an <see langword="IEnumerable"/> to the Blind
    ///     Carbon Copy (BCC) field of the current <see cref="IEmail"/> 
    ///     instance.
    /// </summary>
    /// <param name="addresses">
    ///     The <see langword="IEnumerable"> who's elements should be added to
    ///     the Blind Carbon Copy (BCC) field of the current
    ///     <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Bcc(IEnumerable<MailAddress> addresses);

    /// <summary>
    ///     Adds a new email address to the Reply To field of the current
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to add.
    /// </param>
    /// <param name="displayName">
    ///     An optional <see cref="string"/> containing the display name to use
    ///     for the email address.  If <see langword="null"/> is provided, then
    ///     the <paramref name="emailAddress"/> value will be used.
    /// </param>
    /// <returns></returns>
    IEmail ReplyTo(string emailAddress, string? displayName = default);

    /// <summary>
    ///     Adds a <see cref="MailAddress"/> instance to the Reply To field of
    ///     the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to add to the Reply To field
    ///     of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail ReplyTo(MailAddress address);

    /// <summary>
    ///     Adds the elements of an <see langword="IEnumerable"/> to the Reply
    ///     To field of the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="addresses">
    ///     The <see langword="IEnumerable"> who's elements should be added to
    ///     the Reply To field of the current <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail ReplyTo(IEnumerable<MailAddress> addresses);

    /// <summary>
    ///     Sets the Subject field of the message for hte current
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="subject">
    ///     A <see cref="string"/> containing the contents of the Subject.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Subject(string subject);

    /// <summary>
    ///     Sets the HTML body of the message for the current 
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="body">
    ///     A <see cref="string"/> containing the contents of the HTML body.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail HtmlBody(string body);

    public IEmail HtmlBody<T>(string path, T? model);

    /// <summary>
    ///     Sets the plain text body of the message for the current
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="body">
    ///     A <see cref="string"/> containing the contents of the plain text
    ///     body.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail PlainTextBody(string body);

    IEmail PlainTextBody<T>(string path, T? model);

    /// <summary>
    ///     Adds a new attachment to the current <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="filePath">
    ///     A <see cref="string"/> containing the absolute file path to the 
    ///     file to add as an attachment.
    /// </param>
    /// <param name="attachmentName">
    ///     An optional <see cref="string"/> containing the display name
    ///     to use for the attachment instead of the file name.
    /// </param>
    /// <param name="mediaType">
    ///     <para>
    ///         A <see cref="string"/> that contains the MIME  Media Type name 
    ///         for the contents in the <paramref name="stream"/>.
    ///     </para>
    ///     <para>
    ///         If <see langword="null"/> is given, media type will be 
    ///         determined by attachment name.  
    ///     </para>
    ///     <para>
    ///         If media type cannot be determined, "application/octet" will be
    ///         used.
    ///     </para>
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Attach(string filePath, string? attachmentName = default, string? mediaType = default);

    /// <summary>
    ///     Attaches a new <see cref="Attachment"/> to the current 
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="stream">
    ///     A readable <see cref="Stream"/> that contains the contents for this
    ///     attachment.
    /// </param>
    /// <param name="attachmentName">
    ///     A <see cref="string"/> that contains the display name to give this
    ///     attachment, including the extension (e.g. img.png).
    /// </param>
    /// <param name="mediaType">
    ///     <para>
    ///         A <see cref="string"/> that contains the MIME  Media Type name 
    ///         for the contents in the <paramref name="stream"/>.
    ///     </para>
    ///     <para>
    ///         If <see langword="null"/> is given, media type will be 
    ///         determined by attachment name.  
    ///     </para>
    ///     <para>
    ///         If media type cannot be determined, "application/octet" will be
    ///         used.
    ///     </para>
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Attach(Stream stream, string attachmentName, string? mediaType = default);

    /// <summary>
    ///     Attaches the given <see cref="Attachment"/> to the current
    ///     <see cref="IEmail"/> instance.
    /// </summary>
    /// <param name="attachment">
    ///     A instance of the <see cref="Attachment"/> class to attach to this
    ///     <see cref="IEmail"/> instance.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    IEmail Attach(Attachment attachment);

    /// <summary>
    ///     Synchronously sends the current <see cref="IEmail"/> instance using
    ///     the <see cref="IEmailSender"/> specified during initialization.
    /// </summary>
    /// <remarks>
    ///     This instance will be disposed of automatically after sending.
    /// </remarks>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="SendResult"/> class that represents
    ///     the result of sending this email.
    /// </returns>
    SendResult Send(CancellationToken token = default(CancellationToken));


    /// <summary>
    ///     Asynchronously sends the current <see cref="IEmail"/> instance using
    ///     the <see cref="IEmailSender"/> specified during initialization.
    /// </summary>
    /// <remarks>
    ///     This instance will be disposed of automatically after sending.
    /// </remarks>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     A new <see cref="Task"/> object that contains the results of this
    ///     asynchronous operation.
    /// </returns>
    Task<SendResult> SendAsync(CancellationToken token = default(CancellationToken));
}