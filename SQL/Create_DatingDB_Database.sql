USE master;  
GO  

CREATE DATABASE DatingDB  
ON   
( NAME = DatingDB_data,  
    FILENAME = 'C:\SQL Databases\DatingDB_data.mdf',  
    SIZE = 10,  
    FILEGROWTH = 5 )  
LOG ON  
( NAME = DatingDB_log,  
    FILENAME = 'C:\SQL Databases\DatingDB_log.ldf',  
    SIZE = 5MB,  
    FILEGROWTH = 5MB );  
GO  