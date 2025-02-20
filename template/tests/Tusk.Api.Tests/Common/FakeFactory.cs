using AutoMapper;
using MediatR;
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

    public static Mapper GetMapper(IEnumerable<Profile> profiles)
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfiles(profiles);
        });
        return new Mapper(configuration);
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
