<h1>MVC - Mirth Version Control</h1>

![GitHub release (latest by date)](https://img.shields.io/github/v/release/Mysttic/MVC)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Mysttic/MVC?include_prereleases)

![MCVClogo](https://github.com/user-attachments/assets/7aa0d96f-4d1b-4f3f-bb56-ddb2bc5abdf1)

**Allow you to backup your Mirth Connect Configurations with full changes history**

<h2>About this Project</h2>

With this tool, you will be able to monitor what changes occur in the configuration saved in the database. 
An independently running application, after connecting to the database, monitors the tables, 
and after a change occurs, saves the configuration of a given record to the repository directory.

Tables included for version control checking:
- Channel
- Code_template

Supported databases:
- MSSQL
- PostgreSQL
- MySQL
- Oracle

This application is a lighter version of [this](https://github.com/Mysttic/MirthConnectVersionControl) solution where you can manage all instances in one place. 
This solution focuses solely on separating the dependencies of the operation for separate types of databases, 
easier startup and the possibility of containerization.

<h2>How to use</h2>
The application can be used in 3 different configurations:

<h3>User Interactive</h3>
We can run it directly from the console by passing the run parameters as an arguments:

- /console - runs the application in console mode
- /connection string - access to the database to listen to
- /logpath - location where logs are saved
- /repopath - location where versions of changes are saved
- /usegit - (default false) - specifies whether we want to use the git service for the change history or the option to save individual versions in separate files
- /gitchannels (default true) - allow to turn on/off checking version control for Channels
- /gitcodetemplates (default true) - allow to turn on/off checking version control for Code templates

Example startup script:

```
 mvc.mssqllistener.exe /console /connectionstring="TrustServerCertificate=True;User ID=sa;Password=sa;Initial Catalog=mirthdb;Data Source=localhost" /logpath="C:\Logs" /repopath="C:\Repo" /usegit=true
```

<h3>Service</h3>
We can install the application as a service. To do this, you need to install it from the file included with the specific solution.
You need to find and run the **install.cmd** file as administrator.

This file must be run in the directory where the .exe file of the service being installed is located. The script is universal for all listeners and automatically detects the file to be installed.

![image](https://github.com/user-attachments/assets/90d44bd9-8336-4b69-b5e6-8995a7e85184)

Running the installer, we will need to provide the application startup parameters. 
After providing them and clicking Enter, the script will continue with the installation process. 
If the service was previously installed, the script will try to uninstall it. 
When the service is installed, it will start with the parameters provided.

![image](https://github.com/user-attachments/assets/08132662-0ed5-42e1-985b-6555d8a74d8a)


<h3>Docker</h3>

Main purpose of this solution was to deliver light version of MVC that can be hosted on docker container. You can download proper version from docker hub [here](https://hub.docker.com/u/mysttic) , or by using this script (_mssql, mysql, postgresql, oracle_):

```
docker pull mysttic/mvc.{database}listener
```

When you download the image, you can run the container using code below (adapted to your parameters ofc).

```
docker run mvc.postgresqllistener --build-arg /connectionstring="Host=localhost;Port=5432;Database=mirthdb;Username=mirthdb;Password=mirthdb" /logpath="app/Logs" /repopath="app/Repo" /usegit=true
```


Logs and Repo files would be stored by default inside the container, but if you want you can store them on host directory by mounting it to the container ([more info here](https://docs.docker.com/storage/bind-mounts/))

Below example:

```
docker run -v /host_mnt/c/MVC:/app/MVC mvc.postgresqllistener --build-arg /connectionstring="Host=localhost;Port=5432;Database=mirthdb;Username=mirthdb;Password=mirthdb" /logpath="/app/MVC/Logs" /repopath="/app/MVC/Repo" /usegit=true
```

Just remember to adjust this directory in further parameters (logpath and repopath).

##### And very important reminder, these containers were hosted on LINUX, so remember about catalogue structures and restrictions.

<h2>Disclaimer</h2>
This project and its creators are not affiliated with NextGen Healthcare in any way. 

The solution is an independent application not linked to the Mirth Connect product source code. 

For more information on regulations, please see the licensing arrangements.
