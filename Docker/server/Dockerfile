# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY publish/ App/
WORKDIR /App
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "AlcatrazGameServices.dll"]