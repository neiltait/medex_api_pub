FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
#EXPOSE 57923
EXPOSE 61006

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY MedicalExaminer.ToolBox.MVC/MedicalExaminer.ToolBox.MVC.csproj MedicalExaminer.ToolBox.MVC/
RUN dotnet restore MedicalExaminer.ToolBox.MVC/MedicalExaminer.ToolBox.MVC.csproj
COPY . .
WORKDIR /src/MedicalExaminer.ToolBox.MVC
RUN dotnet build MedicalExaminer.ToolBox.MVC.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish MedicalExaminer.ToolBox.MVC.csproj -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MedicalExaminer.ToolBox.MVC.dll"]
