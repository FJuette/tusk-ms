using System;
using Tusk.Api.Infrastructure;

namespace Tusk.Api.Tests.Common
{
    public static class FakeFactory
    {
        public static IDateTime GetDtInstance()
        {
            return new TestDateTime();
        }
    }

    public class TestDateTime : IDateTime
    {
        public DateTime Now => new DateTime(2020, 05, 27, 11, 11, 11);
    }
}
