using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using DiTraining.DiContainer;

namespace DiTraining.Tests.DiContainer;

[Collection("Sequential")]
public class DiContainerTests
{
    [Fact]
    public void ServiceProvider_Should_Resolve_NotificationService_And_Send_Notification()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IMessageSender, EmailSender>();
        services.AddTransient<NotificationService>();

        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<NotificationService>();

        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);

        try
        {
            // Act
            service.Notify("Hello DiContainer");

            // Assert
            var output = sw.ToString().Trim();
            Assert.Equal("[Email] Hello DiContainer", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
