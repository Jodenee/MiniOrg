using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using TestApi;
using TestApi.Data;
using TestApi.Interfaces;
using TestApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add repositories

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeReviewRepository, EmployeeReviewRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

// Ratelimiting middleware

builder.Services.AddRateLimiter(ratelimitOptions =>
{
	ratelimitOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

	ratelimitOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
		RateLimitPartition.GetTokenBucketLimiter(
			partitionKey: httpContext.Request.Headers.Host.ToString(),
			factory: partition => new TokenBucketRateLimiterOptions
			{
				TokenLimit = 3,
				QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
				QueueLimit = 0,
				ReplenishmentPeriod = TimeSpan.FromSeconds(1),
				TokensPerPeriod = 5,
				AutoReplenishment = true
			}
		)
	);

	ratelimitOptions.OnRejected = async (rejectedCtx, token) =>
	{
		bool retryAfterExists = rejectedCtx.Lease
			.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter);

		if (retryAfterExists)
			rejectedCtx.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString();
		else
			rejectedCtx.HttpContext.Response.Headers.RetryAfter = "1";

		await rejectedCtx.HttpContext.Response.WriteAsync(
			"You have been ratelimited! Please do keep in mind our 3 requests per second limit. " +
			"Read about ratelimits here https://www.cloudflare.com/en-gb/learning/bots/what-is-rate-limiting/",
			token
		);
	};
});

// Add output caching middleware

builder.Services.AddOutputCache(options =>
{
	options.AddBasePolicy(builder =>
	{
		builder.Expire(TimeSpan.FromSeconds(10));
		builder.SetVaryByQuery(["*"]);
	});

	options.AddPolicy("Expire30s", builder =>
	{
		builder.Expire(TimeSpan.FromSeconds(30));
		builder.SetVaryByQuery(["*"]);
	});
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<DataContext>(options =>
	options
		.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

// Seed the db

if (args.Length == 1 && args[0] == "seeddb")
{
	IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>()!;

	using IServiceScope scope = scopeFactory.CreateScope();

	Seed seedService = scope.ServiceProvider.GetService<Seed>()!;
	await seedService.SeedDatabase();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseResponseCaching();

app.UseOutputCache();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
