using PastaFit.Features.Booking.Adapters;
using PastaFit.Features.Booking.Ports;
using PastaFit.Shell.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var inMemoryBookingAdapter = new InMemoryBookingRepository();
inMemoryBookingAdapter.Bootstrap();
builder.Services.AddSingleton<ICreateBookingRepository>(inMemoryBookingAdapter);
builder.Services.AddSingleton<ICancelBookingRepository>(inMemoryBookingAdapter);
IServiceCollection temp = builder.Services;

var app = builder.Build();

app.MapBookingEndpoints();

app.Run();

public partial class Program { }
