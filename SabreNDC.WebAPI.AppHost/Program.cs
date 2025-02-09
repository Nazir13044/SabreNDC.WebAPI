var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SabreNDC_WebAPI>("sabrendc-webapi");

builder.Build().Run();
