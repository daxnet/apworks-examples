FROM microsoft/dotnet:latest
RUN mkdir /var/tasklist
WORKDIR /var/tasklist
COPY publish/*.* ./
EXPOSE 5000
CMD ["dotnet", "Apworks.Examples.TaskList.dll"]
