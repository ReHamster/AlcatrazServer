# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY publish/ App/
WORKDIR /App
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 21005/udp
EXPOSE 21006/udp
ENTRYPOINT ["dotnet", "AlcatrazGameServices.dll"]