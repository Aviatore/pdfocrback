FROM mcr.microsoft.com/dotnet/aspnet:5.0 As base
EXPOSE 4200
COPY PdfOcr/bin/Release/net5.0/publish/ App/

RUN apt update
RUN apt install -y libgdiplus ocrmypdf tesseract-ocr tesseract-ocr-pol nginx

COPY nginx.conf /etc/nginx/nginx.conf
COPY /dist/pdfOcr /usr/share/nginx/html
COPY set_environment.sh /set_environment.sh

FROM base
WORKDIR /App
ENTRYPOINT ["bash", "/set_environment.sh"]
