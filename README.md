# Exam Project

## Docker Command Examples
- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin@123$" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
- docker ps or docker container ls
- docker run -d --name mongodb -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Admin@123$ -p 0.0.0.0:27018:27017 mongo