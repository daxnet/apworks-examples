FROM microsoft/dotnet:latest
RUN mkdir /app
WORKDIR /app
COPY publish/ ./
EXPOSE 5000
CMD ["dotnet", "Apworks.Examples.TaskList.dll"]
