FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
#EXPOSE 57923
EXPOSE 44371

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY MedicalExaminer.API/MedicalExaminer.API.csproj MedicalExaminer.API/
RUN dotnet restore MedicalExaminer.API/MedicalExaminer.API.csproj
COPY . .
WORKDIR /src/MedicalExaminer.API
RUN dotnet build MedicalExaminer.API.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish MedicalExaminer.API.csproj -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MedicalExaminer.API.dll"]
