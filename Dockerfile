FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
COPY /app/publish/KamaVerification.UI.Core /app/
EXPOSE 80

ENTRYPOINT ["dotnet", "KamaVerification.UI.Core.dll"]