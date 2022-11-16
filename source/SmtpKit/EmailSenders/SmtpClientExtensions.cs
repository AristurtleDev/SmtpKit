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
///     Extensions methods for the <see cref="SmtpClient"/> class.
/// </summary>
internal static class SmtpClientExtentions
{
    /// <summary>
    ///     Asynchronously sends the specified <see cref="MailMessage"/> 
    ///     instance to an SMTP server for delivery while preventing deadlocks
    ///     when an exception occurs during deliver causing the asynchronous
    ///     operation to hang.
    /// </summary>
    /// <remarks>
    ///     see <see href="https://stackoverflow.com/questions/28333396/smtpclient-sendmailasync-causes-deadlock-when-throwing-a-specific-exception/28445791#28445791"/>
    /// </remarks>
    /// <param name="client">
    ///     The instance of <see cref="SmtpClient"/> used to send the 
    ///     <paramref name="message"/> to the SMTP server for delivery.
    /// </param>
    /// <param name="message">
    ///     The <see cref="MailMessage"/> instance to send to the SMTP server
    ///     for delivery.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     A new <see cref="Task"/> object containing the result of this
    ///     asynchronous operation.
    /// </returns>
    public static Task SendMailAsyncNoDeadlock(this SmtpClient client, MailMessage message, CancellationToken token = default(CancellationToken))
    {
        //  Task.Run used to negate SynchronizationContext so that exceptions
        //  will bubble up properly.
        return Task.Run(() => SendAsync(client, message, token));
    }

    /// <summary>
    ///     Asynchronously sends the specified <see cref="MailMessage"/> 
    ///     instance to an SMTP server for delivery while preventing deadlocks
    ///     when an exception occurs during deliver causing the asynchronous
    ///     operation to hang.
    /// </summary>
    /// <remarks>
    ///     see <see href="https://stackoverflow.com/questions/28333396/smtpclient-sendmailasync-causes-deadlock-when-throwing-a-specific-exception/28445791#28445791"/>
    /// </remarks>
    /// <param name="client">
    ///     The instance of <see cref="SmtpClient"/> used to send the 
    ///     <paramref name="message"/> to the SMTP server for delivery.
    /// </param>
    /// <param name="message">
    ///     The <see cref="MailMessage"/> instance to send to the SMTP server
    ///     for deliver.
    /// </param>
    /// <param name="token">
    ///     The token to monitor for cancellation requests.  Uses 
    ///     <see cref="CancellationToken.None"/> if one is not provided.
    /// </param>
    /// <returns>
    ///     A new <see cref="Task"/> object containing the result of this
    ///     asynchronous operation.
    /// </returns>
    private static async Task SendAsync(SmtpClient client, MailMessage message, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        TaskCompletionSource<bool> completion = new();
        SendCompletedEventHandler? handler = default;
        Action unsubscribe = () => client.SendCompleted -= handler;

        handler = async (sender, args) =>
        {
            unsubscribe();

            await Task.Yield();

            if (args.UserState != completion)
            {
                completion.TrySetException(new InvalidOperationException($"Unexpected UserState '{args.UserState}'"));
            }
            else if (args.Cancelled)
            {
                completion.TrySetCanceled();
            }
            else if (args.Error is Exception ex)
            {
                completion.TrySetException(ex);
            }
            else
            {
                completion.TrySetResult(true);
            }
        };

        client.SendCompleted += handler;

        try
        {
            client.SendAsync(message, completion);
            using (token.Register(() => { client.SendAsyncCancel(); }, useSynchronizationContext: false))
            {
                await completion.Task;
            }
        }
        finally
        {
            unsubscribe();
        }
    }
}