![GitHub release (latest by date)](https://img.shields.io/github/v/release/Mysttic/MVC)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/Mysttic/MVC?include_prereleases)


<h1>MVC - Mirth Version Control</h1>

![MCVClogo](https://github.com/user-attachments/assets/7aa0d96f-4d1b-4f3f-bb56-ddb2bc5abdf1)

**Allow you to backup your Mirth Connect Channels with full changes history**

<h2>About this Project</h2>

With this tool, you will be able to monitor what changes occur in the configuration of channels saved in the database. 
An independently running application, after connecting to the database, monitors the table with channels, 
and after a change occurs, saves the configuration of a given channel to the repository directory.

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
- /usegit - (default false) specifies whether we want to use the git service for the change history or the option to save individual versions in separate files

Example startup script:

 _mvc.mssqllistener.exe /connectionstring="TrustServerCertificate=True;User ID=sa;Password=sa;Initial Catalog=mirthdb;Data Source=localhost" /logpath="C:\Logs" /repopath="C:\Repo" /usegit=true_

 <h2>Disclaimer</h2>
This project and its creators are not affiliated with NextGen Healthcare in any way. 

The solution is an independent application not linked to the Mirth Connect product source code. 

For more information on regulations, please see the licensing arrangements.
