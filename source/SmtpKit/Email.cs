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
using System.Net.Mime;
using System.Text;

using SmtpKit.EmailRenderers;
using SmtpKit.EmailSenders;

namespace SmtpKit;

public sealed class Email : IEmail
{
    //  This is the mail message that is being build by this instance
    private MailMessage _message;

    private IEmailSender _sender;
    private IEmailRenderer _renderer;

    private Email(MailAddress from, IEmailSender sender)
    {
        // DataFields = new(from);
        _sender = sender;

        _message = new()
        {
            From = from
        };

        _renderer = new ReplacementRenderer();
    }

    /// <summary>
    ///     Creates a new <see cref="Email"/> instance with the specified
    ///     <paramref name="emailAddress"/> set to the From field, and using the
    ///     specified <paramref name="sender"/> to send the email message with.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to set to the
    ///     From field of the <see cref="Email"/> instance created.
    /// </param>
    /// <param name="displayName">
    ///     A <see cref="string"/> containing the display name to use for the
    ///     email address. 
    /// </param>
    /// <param name="sender">
    ///     The <see cref="IEmailSender"/> instance that will be used to send
    ///     the email message.
    /// </param>
    /// <returns>
    ///     The created <see cref="Email"/> instance as an instance of
    ///     <see cref="IEmail"/>.
    /// </returns>
    public static IEmail Create(string emailAddress, IEmailSender sender) =>
        Create(new MailAddress(emailAddress), sender);

    /// <summary>
    ///     Creates a new <see cref="Email"/> instance with the specified
    ///     <paramref name="emailAddress"/> and <paramref name="displayName"/>
    ///     set to the From field, and using the specified
    ///     <paramref name="sender"/> to send the email message with.
    /// </summary>
    /// <param name="emailAddress">
    ///     A <see cref="string"/> containing the email address to set to the
    ///     From field of the <see cref="Email"/> instance created.
    /// </param>
    /// <param name="displayName">
    ///     A <see cref="string"/> containing the display name to use for the
    ///     email address. 
    /// </param>
    /// <param name="sender">
    ///     The <see cref="IEmailSender"/> instance that will be used to send
    ///     the email message.
    /// </param>
    /// <returns>
    ///     The created <see cref="Email"/> instance as an instance of
    ///     <see cref="IEmail"/>.
    /// </returns>
    public static IEmail Create(string emailAddress, string displayName, IEmailSender sender) =>
        Create(new MailAddress(emailAddress, displayName), sender);

    /// <summary>
    ///     Creates a new <see cref="Email"/> instance using the specified
    ///     <see cref="MailAddress"/> as the From field and the specified
    ///     <see cref="IEmailSender"/> to send the email message with.
    /// </summary>
    /// <param name="address">
    ///     The <see cref="MailAddress"/> instance to set as the From field
    ///     of the <see cref="Email"/> instance. created.
    /// </param>
    /// <param name="sender">
    ///     The <see cref="IEmailSender"/> instance that will be used to send
    ///     the email message.
    /// </param>
    /// <returns>
    ///     The created <see cref="Email"/> instance as an instance of
    ///     <see cref="IEmail"/>.
    /// </returns>
    public static IEmail Create(MailAddress from, IEmailSender sender) => new Email(from, sender);

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
    public IEmail From(string emailAddress, string? displayName = default) =>
        From(new MailAddress(emailAddress, emailAddress));

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
    public IEmail From(MailAddress address)
    {
        _message.From = address;
        return this;
    }

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
    public IEmail To(string emailAddress, string? displayName = default) =>
        To(new MailAddress(emailAddress, displayName));

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
    public IEmail To(MailAddress address)
    {
        _message.To.Add(address);
        return this;
    }

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
    public IEmail To(IEnumerable<MailAddress> addresses)
    {
        _message.To.AddRange(addresses);
        return this;
    }

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
    public IEmail Cc(string emailAddress, string? displayName = default) =>
        Cc(new MailAddress(emailAddress, displayName));

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
    public IEmail Cc(MailAddress address)
    {
        _message.CC.Add(address);
        return this;
    }

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
    public IEmail Cc(IEnumerable<MailAddress> addresses)
    {
        _message.CC.AddRange(addresses);
        return this;
    }

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
    public IEmail Bcc(string emailAddress, string? displayName = default) =>
        Bcc(new MailAddress(emailAddress, displayName));

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
    public IEmail Bcc(MailAddress address)
    {
        _message.Bcc.Add(address);
        return this;
    }

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
    public IEmail Bcc(IEnumerable<MailAddress> addresses)
    {
        _message.Bcc.AddRange(addresses);
        return this;
    }

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
    public IEmail ReplyTo(string emailAddress, string? displayName = default) =>
        ReplyTo(new MailAddress(emailAddress, displayName));

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
    public IEmail ReplyTo(MailAddress address)
    {
        _message.ReplyToList.Add(address);
        return this;
    }

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
    public IEmail ReplyTo(IEnumerable<MailAddress> addresses)
    {
        _message.ReplyToList.AddRange(addresses);
        return this;
    }

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
    public IEmail Subject(string subject)
    {
        _message.Subject = subject;
        return this;
    }

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
    public IEmail HtmlBody(string body)
    {
        if (_message.AlternateViews.Count > 0)
        {
            _message.AlternateViews[0].Dispose();
            _message.AlternateViews.Clear();
        }

        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body);
        htmlView.ContentType = new("text/html; charset=UTF-8");
        _message.AlternateViews.Add(htmlView);
        _message.IsBodyHtml = true;

        return this;
    }

    public IEmail HtmlBody<T>(string path, T? model)
    {
        if (_message.AlternateViews.Count > 0)
        {
            _message.AlternateViews[0].Dispose();
            _message.AlternateViews.Clear();
        }

        string body = File.ReadAllText(path);
        body = _renderer.Render<T>(body, model);

        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body);
        htmlView.ContentType = new("text/html; charset=UTF-8");
        _message.AlternateViews.Add(htmlView);
        _message.IsBodyHtml = true;

        return this;
    }

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
    public IEmail PlainTextBody(string body)
    {
        _message.Body = body;
        _message.BodyEncoding = Encoding.UTF8;
        return this;
    }

    public IEmail PlainTextBody<T>(string path, T? model)
    {
        string body = File.ReadAllText(path);
        body = _renderer.Render<T>(body, model);
        _message.Body = body;
        _message.BodyEncoding = Encoding.UTF8;
        return this;
    }

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
    public IEmail Attach(string filePath, string? attachmentName = default, string? mediaType = default)
    {
        if (mediaType is null)
        {
            mediaType = GetMIMEType(Path.GetExtension(filePath));
        }

        Attachment attachment = new(filePath, mediaType);

        if (attachment.ContentDisposition is ContentDisposition disposition)
        {
            disposition.FileName = attachmentName ?? Path.GetFileName(filePath);
            disposition.Inline = false;
            disposition.CreationDate = File.GetCreationTimeUtc(filePath);
            disposition.ModificationDate = File.GetLastWriteTimeUtc(filePath);
            disposition.ReadDate = File.GetLastAccessTimeUtc(filePath);
        }

        return Attach(attachment);
    }

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
    public IEmail Attach(Stream stream, string attachmentName, string? mediaType = default)
    {
        if (mediaType is null)
        {
            mediaType = GetMIMEType(Path.GetExtension(attachmentName));
        }

        Attachment attachment = new(stream, mediaType);

        if (attachment.ContentDisposition is ContentDisposition disposition)
        {
            disposition.FileName = attachmentName;
            disposition.Inline = false;
            disposition.CreationDate = DateTime.UtcNow;
            disposition.ModificationDate = DateTime.UtcNow;
            disposition.ReadDate = DateTime.UtcNow;
        }

        return Attach(attachment);
    }

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
    public IEmail Attach(Attachment attachment)
    {
        _message.Attachments.Add(attachment);
        return this;
    }

    /// <summary>
    ///     Synchronously sends the current <see cref="IEmail"/> instance using
    ///     the <see cref="IEmailSender"/> specified during initialization.
    /// </summary>
    /// <param name="token">
    /// 
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    public void Send(CancellationToken token = default(CancellationToken)) => _sender.Send(_message, token);


    /// <summary>
    ///     Asynchronously sends the current <see cref="IEmail"/> instance using
    ///     the <see cref="IEmailSender"/> specified during initialization.
    /// </summary>
    /// <param name="token">
    /// 
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IEmail"/>.
    /// </returns>
    public Task SendAsync(CancellationToken token = default(CancellationToken)) => _sender.SendAsync(_message, token);

    /// <summary>
    ///     Returns a new <see cref="string"/> containing the representation of
    ///     the current state of this <see cref="Email"/>.
    /// </summary>
    /// <returns>
    ///     A new <see cref="string"/> containing the representation of the
    ///     current state of this <see cref="Email"/>.
    /// </returns>
    public override string ToString() =>
    $"""
    FROM: {_message.From?.ToString()}
    TO: {string.Join(';', _message.To)}
    CC: {string.Join(';', _message.CC)}
    BCC: {string.Join(';', _message.Bcc)}
    REPLY_TO: {string.Join(';', _message.ReplyToList)}

    SUBJECT: {_message.Subject}

    {_message.Body}

    {_message.AlternateViews.FirstOrDefault()?.ToString()}
    """;

    /// <summary>
    ///     Given a <see cref="string"/> containing a file extension, returns
    ///     the MIME media type string for that file extension.
    /// </summary>
    /// <param name="ext">
    ///     A <see cref="string"/> containing the extension of the file,
    ///     including the leading '.', to  get the MIME media type of.
    /// </param>
    /// <returns>
    ///     A new <see cref="string"/> containing the MIME media type string.
    /// </returns>
    private static string GetMIMEType(string? ext) => ext switch
    {
        #pragma warning disable format
        //  null or empty check first so we don't' evaluate every case just
        //  to realize it's just a null or empty string
        null or "" => "application/octet-stream",

        //  Common MIME types for the web.
        //  List gathered from https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        ".aac"    => "audio/aac",
        ".abw"    => "application/x-abiword",
        ".arc"    => "application/x-freearc",
        ".avif"   => "image/avif",
        ".avi"    => "video/x-msvideo",
        ".azw"    => "application/vnd.amazon.ebook",
        ".bin"    => "application/octet-stream",
        ".bmp"    => "image/bmp",
        ".bz"     => "application/x-bzip",
        ".bz2"    => "application/x-bzip2",
        ".cda"    => "application/x-cdf",
        ".csh"    => "application/x-csh",
        ".css"    => "text/css",
        ".csv"    => "text/csv",
        ".doc"    => "application/msword",
        ".docx"   => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".eot"    => "application/vnd.ms-fontobject",
        ".epub"   => "application/epub+zip",
        ".gz"     => "application/gzip",
        ".gif"    => "image/gif",
        ".htm"    => "text/html",
        ".html"   => "text/html",
        ".ico"    => "image/vnd.microsoft.icon",
        ".ics"    => "text/calendar",
        ".jar"    => "application/java-archive",
        ".jpeg"   => "image/jpeg",
        ".jpg"    => "image/jpeg",
        ".js"     => "text/javascript",
        ".json"   => "application/json",
        ".jsonld" => "application/ld+json",
        ".mid"    => "audio/midi",
        ".midi"   => "audio/midi",
        ".mjs"    => "text/javascript",
        ".mp3"    => "audio/mpeg",
        ".mp4"    => "video/mp4",
        ".mpeg"   => "video/mpeg",
        ".mpkg"   => "application/vnd.apple.installer+xml",
        ".odp"    => "application/vnd.oasis.opendocument.presentation",
        ".ods"    => "application/vnd.oasis.opendocument.spreadsheet",
        ".odt"    => "application/vnd.oasis.opendocument.text",
        ".oga"    => "audio/ogg",
        ".ogv"    => "video/ogg",
        ".ogx"    => "application/ogg",
        ".opus"   => "audio/opus",
        ".otf"    => "font/otf",
        ".png"    => "image/png",
        ".pdf"    => "application/pdf",
        ".php"    => "application/x-httpd-php",
        ".ppt"    => "application/vnd.ms-powerpoint",
        ".pptx"   => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        ".rar"    => "application/vnd.rar",
        ".rtf"    => "application/rtf",
        ".sh"     => "application/x-sh",
        ".svg"    => "image/svg+xml",
        ".tar"    => "application/x-tar",
        ".tif"    => "image/tiff",
        ".tiff"   => "image/tiff",
        ".ts"     => "video/mp2t",
        ".ttf"    => "font/ttf",
        ".txt"    => "text/plain",
        ".vsd"    => "application/vnd.visio",
        ".wav"    => "audio/wav",
        ".weba"   => "audio/webm",
        ".webm"   => "video/webm",
        ".webp"   => "image/webp",
        ".woff"   => "font/woff",
        ".woff2"  => "font/woff2",
        ".xhtml"  => "application/xhtml+xml",
        ".xls"    => "application/vnd.ms-excel",
        ".xlsx"   => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".xml"    => "application/xml",
        ".xul"    => "application/vnd.mozilla.xul+xml",
        ".zip"    => "application/zip",
        ".3gp"    => "video/3gpp; audio/3gpp if it doesn't contain video",
        ".3g2"    => "video/3gpp2; audio/3gpp2 if it doesn't contain video",
        ".7z"     => "application/x-7z-compressed",
        _         => "application/octet-stream"

    #pragma warning restore format
    };



}