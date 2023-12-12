using GraphQL;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>(opt => opt.UseSqlServer("Server=(local)\\SQLEXPRESS;Database=GRAPHQL;Integrated Security=SSPI;TrustServerCertificate=True"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddDefaultTransactionScopeHandler()
                .AddMutationConventions(applyToAllMutations: true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGraphQL();

app.Run();
