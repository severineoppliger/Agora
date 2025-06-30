FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN dotnet tool install -g docfx

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /_site
COPY _site/ .

EXPOSE 8080

CMD ["docfx", "serve", ".", "--hostname", "0.0.0.0", "--port", "8080"]
