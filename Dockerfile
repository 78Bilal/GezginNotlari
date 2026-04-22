# Build aşaması
FROM ://microsoft.com AS build-env
WORKDIR /app

# Proje dosyalarını kopyala ve restore et
COPY *.csproj ./
RUN dotnet restore

# Kaynak kodları kopyala ve yayınla (publish)
COPY . ./
RUN dotnet publish -c Release -o out

# Çalıştırma aşaması
FROM ://microsoft.com
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GezginNotlari.dll"]
