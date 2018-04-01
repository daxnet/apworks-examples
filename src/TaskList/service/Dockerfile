FROM microsoft/aspnetcore:2.0.0
RUN mkdir /app
WORKDIR /app
COPY publish/ ./
EXPOSE 9023
CMD ["dotnet", "Apworks.Examples.TaskList.dll"]
