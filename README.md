<h1 align="center">
<img src="https://raw.githubusercontent.com/AristurtleDev/SmtpKit/main/.github/images/smtpkit.png" alt="SmtpKit Logo">
<br/>
A Small .NET Library For Sending Emails Using SMTP
</h1>

SmtpKit is a small .NET library for sending emails using SMTP, featuring a fluent syntax for creating and sending the email messages.  Can be used both with and without dependency injection.

## Basic Usage (Without Dependency Injection)

```csharp
//  Create a sender
IEmailSender sender = new SmtpSender("host", 25);

//  Create and send email
Email.Create("from@address.com", smtp)
     .To("to@address.com")
     .Subject("Hello World")
     .PlainTextBody("How are you today?")
     .HtmlBody("<p>How are you today?</p>")
     .Send();
```

Each email created must be given an **email address** that the email is from and an `IEmailSender` instance which is used to perform the delivery of the email.  In the example above, we are using an instance of the `SmtpSender` which will send the email message to an SMTP sever for delivery.  There may be times, however, when this is not ideal.  For instance, if you are on a local development environment which doesn't have access to communicate with the SMTP server.  In those instance, two other `IEmailSender` implementations are available. 

| Name | Description | Example Usage |
| ---- | ----------- | ------------- |
| `ConsoleSender` | Writes the contents of the email message built to the console window | `IEmailSender sender = new Console()` |
| `FileSystemSender` | Writes the contents of the email message to a text file in a specified directory | `IEmailSender sender = new FileSystemSender("path/to/output/directory")` |

<br />

> 💡 **Note**:  
> After calling `Send()`, the `IEmail` instance that is created will internally dispose of itself after the message is sent.  This is to release any resources that are being held by attachments added to the email.


## Dependency Injection
SmtpKit comes with extension methods to easily add it as a dependency in your service container for dependency injection.  By doing this, it will inject an instance of `IEmailFactory` which can be used each time you need to create and send an email message.  The following example demonstrates how to add it to your services container

```csharp
services.AddSmtpKit("from@address.com")
        .UseSmtp("host", 25);
```

The example above shows how to add it to your services container using SMTP as the delivery method.  Just like it was mentioned in the [Basic Usage](#basic-usage-without-dependency-injection) section above, there may be times when you are developing but can't connect to your SMTP server.  In these situations, you can tell the service to use a `ConsoleSender` or `FileSystemSender` by calling the appropriate `UseConsole()` or `UseFileSystem(string)` methods instead of `UseSmtp(string, int)`.

<br />

> 💡 **Note**:  
> You can only specify to use one of smtp, console, or file system for the service added.


After adding it to your services container, in your code where the `IEmailFactory` instance will be injected (e.g. Razor PageModel), you call the `Create()` method from the `IEmailFactory` instance to create the email instance, which you can then use the fluent syntax to create and send it.  The following is an example of this in a Razor PageModel:

```csharp
public class MyPageModel : PageModel
{
    private readonly IEmailFactory _factory;

    //  IEmailFactory supplied by dependency injection
    public class MyPageModel(IEmailFactory factory) => _factory = factory;

    public async Task OnPostAsync()
    {
        await factory.Create()
                     .To("other@address.com", "Other Person")
                     .Subject("Hello World")
                     .PlainTextBody("How are you today?")
                     .HtmlBody("<p>How are you today?</p>")
                     .SendAsync();        
    }
```

## Using Templates (Experimental)
Templates are currently supported in code but are marked as an experimental feature.  Using templates is a great way to keep your code files clean and make the email templates reusable and dynamic with variables. 

**Template variables** are sections within your template file that are wrapped in double curly brackets.  For instance, consider the following template

```plaintext
Hello {{Name}},
Today is {{CurrentDate}}
```

When using a template, a model `class` instance must be given.  Any **public** properties of that class that have a name that matches the name of the **template variable** will have that property value injected into the template.  For example, if I had the following model:

```csharp
public class TemplateModel
{
    public string Name { get; set; }
    public DateTime DateTime { get; set; }
}
```

The value of the property `TemplateModel.Name` would be injected anywhere in the template where `{{Name}}` appeared,and the value fo the property `TemplateModel.CurrentDate` would be injected anywhere in the template where `{{CurrentDate}}` appeared.

To make use of a template, you use the `.PlainTextBody<T>(string, T?)` or `.HtmlBody<T>(string, T?)` methods like in the following example

```csharp
Email.Create("from@address.com", smtp)
     .To("to@address.com")
     .Subject("Hello World")
     .PlainTextBody<TemplateModel>("path/to/template/file", templateInstance)
     .HtmlBody<TemplateModel>("path/to/template/file", templateInstance)
     .Send();
```

## License
**SmtpKit** is licensed under the **MIT License**.  Please refer to the [LICENSE](LICENSE) file for full license text.
