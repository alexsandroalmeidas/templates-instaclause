using AutoMapper;
using Microsoft.Extensions.Logging;
using Templates.Api.Application.Mappings;

namespace Templates.Api.Tests.TestHelpers
{
    public static class AutoMapperFixture
    {
        public static IMapper GetMapper()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(UserProfile).Assembly);
            }, loggerFactory);

            return mappingConfig.CreateMapper();
        }
    }
}
