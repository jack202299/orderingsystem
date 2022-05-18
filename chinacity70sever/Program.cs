
using chinacity70sever.BLL;
using chinacity70sever.Models;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
// builder = new WebHostBuilder();

builder.Services.AddAntiforgery(option => {
    option.FormFieldName = "AntiforgeryFieldname";
    option.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    option.SuppressXFrameOptionsHeader = false;
});
// Add services to the container.
builder.Services.AddCors(p => p.AddPolicy("*.*", pp => pp.AllowAnyOrigin()));
var connstring = dbManager.Gethosturl("myconnstring");
builder.Services.AddDbContext<china70Context>(option => option.UseMySql(connstring, ServerVersion.AutoDetect(connstring)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   // app.UseHsts();
}


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
