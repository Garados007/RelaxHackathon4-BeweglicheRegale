FROM mcr.microsoft.com/dotnet/sdk:7.0 as builder
WORKDIR /project
COPY . .
RUN dotnet publish --nologo -c Release -o /app \
        src/Regale/Regale.csproj

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /data
COPY --from=builder /app /app
VOLUME [ "/input" ]
VOLUME [ "/output" ]
CMD [ "dotnet", "/app/Regale.dll", "/input", "/output" ]
