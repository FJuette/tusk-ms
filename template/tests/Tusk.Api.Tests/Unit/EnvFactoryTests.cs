using FluentAssertions;
using Tusk.Api.Infrastructure;
using Tusk.Application.Exceptions;
using Xunit;

namespace Tusk.Api.Tests.Unit;

public class EnvFactoryTests
{
    [Fact]
    public void EnvFactory_Default_NotException()
    {
        // Arrange & Act
        var result = EnvFactory.UseSeq();

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void EnvFactory_NullDefault_ThrowException()
    {
        // Act & Assert
        Assert.Throws<MissingEnvException>(EnvFactory.GetSeqUrl);
    }
}
