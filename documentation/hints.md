## Dedicated user in the database

If we want to restrict access to the database to a user who will only have access to monitoring the required tables, 
we can create a dedicated user and give him only the appropriate permissions

Depending on the database used, the configuration may look different, 
but the main goal is to allow reading only for the **channel** and **code_template** tables (new ones may be added in the future)

### PostgreSQL
```sql
CREATE ROLE monitor_user WITH LOGIN PASSWORD 'your_secure_password';

GRANT CONNECT ON DATABASE your_database TO monitor_user;

GRANT USAGE ON SCHEMA public TO monitor_user;

REVOKE ALL ON ALL TABLES IN SCHEMA public FROM monitor_user;

GRANT SELECT ON public.channel, public.code_template TO monitor_user;
```

### MSSQL
```sql
USE YourDatabase;
GO

CREATE LOGIN monitor_user WITH PASSWORD = 'your_secure_password';
GO

CREATE USER monitor_user FOR LOGIN monitor_user;
GO

GRANT SELECT ON dbo.channel TO monitor_user;
GRANT SELECT ON dbo.code_template TO monitor_user;
GO
```

### MySQL
```sql
USE your_database;

CREATE USER 'monitor_user'@'%' IDENTIFIED BY 'your_secure_password';

GRANT SELECT ON your_database.channel TO 'monitor_user'@'%';
GRANT SELECT ON your_database.code_template TO 'monitor_user'@'%';

FLUSH PRIVILEGES;
```

### Oracle
```sql
-- Log in as a user with DBA privileges

CREATE USER monitor_user IDENTIFIED BY your_secure_password
   DEFAULT TABLESPACE users
   TEMPORARY TABLESPACE temp
   QUOTA 10M ON users;

GRANT CONNECT TO monitor_user;

GRANT SELECT ON PUBLIC.channel TO monitor_user;
GRANT SELECT ON PUBLIC.code_template TO monitor_user;
```
