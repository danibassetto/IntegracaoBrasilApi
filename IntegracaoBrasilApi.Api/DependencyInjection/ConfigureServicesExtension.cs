﻿using IntegracaoBrasilApi.Api.Generic;
using IntegracaoBrasilApi.ApiClient.RefitInterfaces;
using IntegracaoBrasilApi.Domain.ApiManagement;
using IntegracaoBrasilApi.Domain.Interfaces;
using IntegracaoBrasilApi.Domain.Interfaces.Repository;
using IntegracaoBrasilApi.Domain.Interfaces.Service;
using IntegracaoBrasilApi.Domain.Services;
using IntegracaoBrasilApi.Infraestructure.Context;
using IntegracaoBrasilApi.Infraestructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IntegracaoBrasilApi.Api.DependencyInjection;

public static class ConfigureServicesExtension
{
    private const string ConfigBrasilApi = "Integrations:BrasilApi";
    public static IServiceCollection ServiceCollection { get; private set; } = new ServiceCollection();
    public static IConfiguration? Configuration { get; private set; }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ServiceCollection = serviceCollection;
        Configuration = configuration;

        AddOptions();
        AddTransient();
        AddSingleton();
        AddSwaggerGen();
        AddCors();
        AddRefitClient();
        AddToken();

        return ServiceCollection;
    }

    public static void AddOptions()
    {
        ServiceCollection.AddOptions();
    }

    public static void AddTransient()
    {
        ServiceCollection.AddTransient<IAuthenticationService, AuthenticationService>();
        ServiceCollection.AddTransient<IUserService, UserService>();
        ServiceCollection.AddTransient<IUserRepository, UserRepository>();
        ServiceCollection.AddTransient<ILegalPersonRegistrationService, LegalPersonRegistrationService>();
        ServiceCollection.AddTransient<IAddressService, AddressService>();

        ServiceCollection.AddTransient<IApiDataService, ApiDataService>();
    }

    public static void AddSingleton()
    {
        ServiceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        ServiceCollection.AddSingleton<IMongoContext, MongoContext>();
    }

    public static void AddRefitClient()
    {
        var baseUrlBrasilApi = Configuration![ConfigBrasilApi];

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters = new[] { new StringEnumConverter() }
            })
        };

        ServiceCollection.AddRefitClient<ILegalPersonRegistrationRefit>(refitSettings).ConfigureHttpClient(c => { c.BaseAddress = new Uri(baseUrlBrasilApi ?? string.Empty); });
        ServiceCollection.AddRefitClient<IAddressRefit>(refitSettings).ConfigureHttpClient(c => { c.BaseAddress = new Uri(baseUrlBrasilApi ?? string.Empty); });
    }

    public static void AddSwaggerGen()
    {
        OpenApiContact contact = new()
        {
            Name = "BrasilApi",
            Url = new Uri("https://github.com/danibassetto")
        };

        const string title = "BrasilApi";

        ServiceCollection.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("pt-br", new OpenApiInfo { Title = title, Version = "pt-br", Contact = contact });
            x.SwaggerDoc("en", new OpenApiInfo { Title = title, Version = "en", Contact = contact });
            x.SwaggerDoc("es", new OpenApiInfo { Title = title, Version = "es", Contact = contact });

            x.DocumentFilter<SwaggerCustomDocumentFilter>();

            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "JWT Authentication",
                Description = "Digitar somente JWT Bearer token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        ServiceCollection.AddSwaggerGenNewtonsoftSupport();
    }

    public static void AddToken()
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        ServiceCollection.AddAuthentication((options) =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(c =>
        {
            c.RequireHttpsMetadata = false;
            c.SaveToken = true;
            c.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKeyJwt.Key)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddCors()
    {
        ServiceCollection.AddCors(options => { options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });
    }
}