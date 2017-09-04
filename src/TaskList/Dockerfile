FROM microsoft/dotnet:2.0.0-runtime
RUN mkdir /app
WORKDIR /app
COPY publish/ ./
EXPOSE 5000
CMD ["dotnet", "Apworks.Examples.TaskList.dll"]
