<h1>MVC - Mirth Version Control</h1>

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
In the startup parameters we pass the following configuration parameters:

- /connection string - mandatory - access to the database to listen to
- /logpath - location where logs are saved
- /repopath - location where versions of changes are saved
- /usegit - (default false) - specifies whether we want to use the git service for the change history or the option to save individual versions in separate files
- /gitchannels (default true) - allow to turn on/off checking version control for Channels
- /gitcodetemplates (default true) - allow to turn on/off checking version control for Code templates

Example startup script:

```
 mvc.mssqllistener.exe /connectionstring="TrustServerCertificate=True;User ID=sa;Password=sa;Initial Catalog=mirthdb;Data Source=localhost" /logpath="C:\Logs" /repopath="C:\Repo" /usegit=true
```

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
