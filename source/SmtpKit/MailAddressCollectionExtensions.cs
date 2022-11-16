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

namespace SmtpKit;

/// <summary>
///     Extensions methods for the <see cref="MailAddressCollection"/> class.
/// </summary>
internal static class MailAddressCollectionExtensions
{
    /// <summary>
    ///     Adds the elements of an <see langword="IEnumerable"/> to this
    ///     <see cref="MailAddressCollection"/> instance.
    /// </summary>
    /// <param name="collection">
    ///     The <see cref="MailAddressCollection"/> instance to add the elements
    ///     of the <see langword="IEnumerable"/> to.
    /// </param>
    /// <param name="addresses">
    ///     The <see langword="IEnumerable"/> that contains the elements to
    ///     add to this <see cref="MailAddressCollection"/> instance.
    /// </param>
    public static void AddRange(this MailAddressCollection collection, IEnumerable<MailAddress> addresses)
    {
        foreach (MailAddress address in addresses)
        {
            collection.Add(address);
        }
    }
}