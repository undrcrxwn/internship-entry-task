using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TicTacToe.WebAPI;

namespace TicTacToe.IntegrationTests;

public class HealthCheckTests(WebApplicationFactory<Program> webApplicationFactory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact(DisplayName = "Health check endpoint returns 200 OK 'Healthy'")]
    public async Task HealthCheck_Returns_Healthy()
    {
        // Arrange
        var client = webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("Healthy");
    }
}