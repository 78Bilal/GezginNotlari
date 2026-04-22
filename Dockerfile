# Build aşaması
FROM ://microsoft.com AS build
WORKDIR /src

# Proje dosyanızı kopyalayın (Proje adınızın GezginNotlari olduğunu varsayıyorum)
COPY ["Seyahat_Gunlugu.csproj", "./"]
RUN dotnet restore

# Tüm dosyaları kopyalayıp yayınlayın
COPY . .
RUN dotnet publish -c Release -o /app

# Çalıştırma aşaması
FROM ://microsoft.com AS runtime
WORKDIR /app
COPY --from=build /app .

# Uygulamanızın portunu belirtin (ASP.NET 8+ varsayılanı 8080'dir)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Seyahat_Gunlugu.dll"]
