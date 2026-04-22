# Build aşaması
FROM ://microsoft.com AS build
WORKDIR /src

# Proje dosyanızı kopyalayın (Görüntüdeki dosya adı kullanıldı)
COPY ["Seyahat_Gunlugu.csproj", "./"]
RUN dotnet restore

# Tüm dosyaları kopyalayıp yayınlayın
COPY . .
RUN dotnet publish "Seyahat_Gunlugu.csproj" -c Release -o /app

# Çalıştırma aşaması
FROM ://microsoft.com AS runtime
WORKDIR /app
COPY --from=build /app .

# Coolify ve .NET 9 varsayılan portu
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Seyahat_Gunlugu.dll"]
