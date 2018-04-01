# Task List
Task List is a small application with which users are able to manage their to-do items and mark some of which as 'Done' when they are really done. This is more like a to-do item checklist which helps users manage their tasks.

![Task List screenshot](https://raw.githubusercontent.com/daxnet/apworks-examples/master/src/TaskList/docs/TaskListScreenshot.png)
 
Task List utilizes the [Apworks](https://github.com/daxnet/apworks-core "Apworks") framework and demonstrated the followings:

- Apworks Data Service
	- MongoDB repository
	- HAL support
	- Server-side pagination
- Angular 4 with TypeScript and Bootstrap 4
	- Components
	- Http Service
	- Exception handling
	- Notification

## How to Run
### Run from Docker
Before you can run the Task List application from Docker, please make sure that you have already installed Docker CE on your machine, then follow the steps below:

1. Start the MongoDB docker container:

	`sudo docker run -d -P --name mongo mongo`

2. Start the TaskList docker container:

	`sudo docker run -d -p 5000:5000 --link mongo daxnet/apworks-examples-tasklist`

3. Open your web browser and navigate to `http://[your_server_name]:5000`, you should be able to see the application running with the above user interface

### Run Locally
You can also clone this repo and run the Task List example locally. Before you can do so, please make sure that your machine meets the following prerequisites:

- git
- [Docker](https://www.docker.com) (Or Docker for Windows, if you are using Windows 10)
- [docker-compose](https://docs.docker.com/compose/) (If you are using Docker for Windows, this is not needed)
- [Powershell for Linux](https://github.com/powershell/powershell#get-powershell) (This is not needed if you are using Windows 10)
- [.NET Core SDK](https://www.microsoft.com/net/download/core)
- [nodejs](https://nodejs.org/en/) (LTS version is preferable)
- [Angular CLI](https://cli.angular.io/)

Follow the steps below to run the example locally.

1. Clone the repo

	`git clone https://github.com/daxnet/apworks-examples`

2. Use `cd` command to switch to `src/TaskList` directory
3. Create the publication

	`powershell -F publish-all.ps1`

4. Bring up everything

	`sudo docker-compose up`

5. Open your web browser and navigate to `http://[your_server_name]:5000`, you should be able to see the application running with the above user interface

### Run in Development
If you want to develop or customize the example, you will need to install the following softwares or dependencies before you can start with your development tasks.

- git
- Visual Studio 2017
- Visual Studio Code
- MongoDB
- [nodejs](https://nodejs.org/en/) (LTS version is preferable)
- [Angular CLI](https://cli.angular.io/)

The steps are as follows:

1. Start MongoDB
2. Open `Apworks.Examples.sln` in Visual Studio 2017
3. Run the `Apworks.Examples.TaskList` ASP.NET Core Web API application
4. Use `cd` command to switch to `src/TaskList/client` directory, and restore the node packages

	`npm install`

5. Execute the command below to start the Task List web app

	`ng serve`

6. Open your web browser and navigate to `http://localhost:4200`, you should be able to see the application running with the above user interface

