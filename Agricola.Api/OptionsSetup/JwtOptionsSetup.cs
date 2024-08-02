using AgricolaApi.Application;
using Microsoft.Extensions.Options;

namespace AgricolaApi.Api;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        // get section Jwt from appsettings.json and set those values to JwtOptions object
        // to be used anywhere in the app
        _configuration.GetSection(SectionName).Bind(options);
    }
}
