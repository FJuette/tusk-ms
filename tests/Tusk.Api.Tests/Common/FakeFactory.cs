using AutoMapper;
using Tusk.Api.Infrastructure;

namespace Tusk.Api.Tests.Common;
public static class TestFactory
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
}

public class TestDateTime : IDateTime
{
    public DateTime Now => new DateTime(2020, 05, 27, 11, 11, 11);
}
