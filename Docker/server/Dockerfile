FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY publish/ app/
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["./AlcatrazGameServices"]