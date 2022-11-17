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
using System.Reflection;
using System.Text;

namespace SmtpKit.EmailRenderers;

public sealed class ReplacementRenderer : IEmailRenderer
{
    /// <summary>
    ///     Synchronously renders the given template.
    /// </summary>
    /// <typeparam name="T">
    ///     The object type of the model if a model is used.
    /// </typeparam>
    /// <param name="template">
    ///     The template to render.
    /// </param>
    /// <param name="model">
    ///     An optional model to use when rendering.
    /// </param>
    /// <param name="isHtml">
    ///     Whether the render output will contain HTML.
    /// </param>
    /// <returns>
    ///     The rendered template as a string.
    /// </returns>
    public string Render<T>(string template, T? model, bool isHtml = true)
    {
        if (model is not null)
        {
            StringBuilder sb = new(template);

            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public;
            foreach (PropertyInfo property in typeof(T).GetProperties(bindings))
            {
                sb.Replace($"{{{{{property.Name}}}}}", property.GetValue(model)?.ToString());
            }

            template = sb.ToString();
        }

        return template;
    }

    /// <summary>
    ///     Asynchronously renders the given template.
    /// </summary>
    /// <typeparam name="T">
    ///     The object type of the model if a model is used.
    /// </typeparam>
    /// <param name="template">
    ///     The template to render.
    /// </param>
    /// <param name="model">
    ///     An optional model to use when rendering.
    /// </param>
    /// <param name="isHtml">
    ///     Whether the render output will contain HTML.
    /// </param>
    /// <returns>
    ///     A new task object containing the result of the asynchronous
    ///     operation.
    /// </returns>
    public Task<string> RenderAsync<T>(string template, T? model, bool isHtml = true) =>
        Task.FromResult(Render(template, model, isHtml));
}