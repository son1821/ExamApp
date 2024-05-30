# Exam Project

##Application Url
- Idnetity STS: https://localhost:5001
- Exam API: https://localhost:5002
- Exam Admin: https://localhost:6001
- Exam Portal: https://localhost:6002
- Identity Admin:https://localhost:6003
## Docker Command Examples
- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin@123$" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
- docker ps or docker container ls
- docker run -d --name mongodb -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Admin@123$ -p 0.0.0.0:27019:27017 mongo


## Packages References
- https://github.com/serilog/serilog/wiki/Getting-Started
- https://github.com/IdentityServer/IdentityServer4.Quickstart.UI