using System.Net;
using FluentAssertions;
using MercerStore.Tests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MercerStore.Tests.Account;

public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AccountControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsRedirectAndSetsCookie()
    {
        // Arrange
        var formData = new Dictionary<string, string>
        {
            { "EmailAddress", "test@example.com" },
            { "Password", "password123" }
        };

        // Act
        var response = await _client.PostAsync("/Account/Login", new FormUrlEncodedContent(formData));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.GetValues("Set-Cookie").First().Should().Contain("OohhCookies");
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsViewWithModelError()
    {
        // Arrange
        var formData = new Dictionary<string, string>
        {
            { "EmailAddress", "wrong@example.com" },
            { "Password", "wrong-password" }
        };

        // Act
        var response = await _client.PostAsync("/Account/Login", new FormUrlEncodedContent(formData));
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("value=\"wrong@example.com\"");
        content.Should().Contain("Wrong email address");
    }

    [Fact]
    public async Task Logout_ReturnsRedirectToHome()
    {
        // Act
        var response = await _client.GetAsync("/Account/Logout");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/", response.Headers.Location?.OriginalString);
    }
}