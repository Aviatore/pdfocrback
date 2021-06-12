FROM mcr.microsoft.com/dotnet/aspnet:5.0 As base
COPY PdfOcr/bin/Release/net5.0/publish/ App/

FROM base
WORKDIR /App
ENTRYPOINT ["dotnet", "PdfOcr.dll"]
