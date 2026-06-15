using DispatchR;
using Mapster;
using Moq;
using Tusk.Api.Infrastructure;
using Tusk.Application;

namespace Tusk.Api.Tests.Common;
public static class FakeFactory
{
    public static IDateTime GetDtInstance()
    {
        return new TestDateTime();
    }

    public static TypeAdapterConfig GetMapper(IEnumerable<IRegister> registers)
    {
        var config = new TypeAdapterConfig();
        foreach (var register in registers)
            register.Register(config);
        return config;
    }

    public static IMediator GetMediatr()
    {
        return new Mock<IMediator>().Object;
    }

    public static IGetClaimsProvider GetClaimsProvider()
    {
        return new TestClaimsProvider();
    }
}

public class TestDateTime : IDateTime
{
    public DateTime Now => new(2020, 05, 27, 11, 11, 11);
}

public class TestClaimsProvider : IGetClaimsProvider
{
    public string UserId { get; } = "Tester";
}
