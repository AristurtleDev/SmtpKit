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
///     contents of a <see cref="MailMessage"/> to a text file.
/// </summary>
public sealed class FileSystemSender : IEmailSender
{
    private readonly string _outDir;

    /// <summary>
    ///     Creates a new instance of the <see cref="FileSystemSender"/> class.
    /// </summary>
    /// <param name="outDir">
    ///     A <see cref="string"/> containing the directory to output the
    ///     contents of each <see cref="MailMessage"/> to.  If the directory
    ///     does not exit, it will be created.
    /// </param>
    public FileSystemSender(string outDir)
    {
        Directory.CreateDirectory(outDir);
        _outDir = outDir;
    }

    /// <summary>
    ///     Synchronously outputs the contents of the given 
    ///     <see cref="MailMessage"/> ot a new file.
    /// </summary>
    /// <param name="email">
    ///     The <see cref="MailMessage"/> instance to put the contents of to
    ///     a new file.
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
    ///     Asynchronously sends the given email.
    /// </summary>
    /// <param name="email">
    ///     The email to send.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     A new task object containing the result of the asynchronous
    ///     operation.
    /// </returns>
    public async Task<SendResult> SendAsync(MailMessage email, CancellationToken token = default(CancellationToken))
    {
        string rand = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{rand}.txt";
        string outputPath = Path.Combine(_outDir, fileName);

        using StreamWriter writer = new(File.OpenWrite(outputPath));

        await writer.WriteLineAsync(email.ToStringExtended());

        return new SendResult();
    }
}