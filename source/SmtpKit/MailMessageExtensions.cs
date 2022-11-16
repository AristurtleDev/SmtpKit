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
using System.Text;

namespace SmtpKit;

/// <summary>
///     Extension methods for the <see cref="MailMessage"/> class.
/// </summary>
internal static class MailMessageExtensions
{

    /// <summary>
    ///     Returns a new <see cref="string"/> containing a formatted text
    ///     representation of a <see cref="MailMessage"/> instance.
    /// </summary>
    /// <param name="message">
    ///     The <see cref="MailMessage"/> instance to create a formatted text
    ///     representation of.
    /// </param>
    /// <returns>
    ///     A new <see cref="string"/> containing a formatted text 
    ///     representation of the specified <paramref name="message"/>.
    /// </returns>
    public static string ToStringExtended(this MailMessage message)
    {
        if(message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        StringBuilder sb = new();
        sb.Append("From: ").AppendLine(message.From?.ToString());
        sb.Append("To: ").AppendLine(message.To.ToString());
        sb.Append("CC: ").AppendLine(message.CC.ToString());
        sb.Append("BCC: ").AppendLine(message.Bcc.ToString());
        sb.Append("Subject: ").AppendLine(message.Subject);
        sb.AppendLine();
        sb.AppendLine("----- Plain Text Body Begin -----");
        sb.AppendLine(message.Body);
        sb.AppendLine("----- Plain Text Body End -----");
      

        string? alt = default;
        if(message.AlternateViews.FirstOrDefault() is AlternateView altView)
        {
            long pos = altView.ContentStream.Position;
            altView.ContentStream.Seek(0, SeekOrigin.Begin);
            using(StreamReader reader = new(altView.ContentStream, leaveOpen: true))
            {
                alt = reader.ReadToEnd();
            }
            altView.ContentStream.Position = pos;
           
           sb.AppendLine();
            sb.AppendLine("----- Alternate Body Begin -----");
            sb.AppendLine(alt);
            sb.AppendLine("----- Alternate Body End -----");
        }

        return sb.ToString();
    }
}