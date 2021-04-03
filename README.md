# Helpers.DataReaderMapper
Library that helps to Convert from IDataReader Type to your desired object type


# Prerequisites
1- .Net framework 4.7.2 or newer versions. (https://dotnet.microsoft.com/download/dotnet-framework) .

2- For Sql Server databases, Run the "DatabaseInitScript.sql" database script found in "Database Script" Solution folder.


# Getting Started
Helpers.DataReaderMapper is a library that is [available on NuGet](https://www.nuget.org/packages/Helpers.DataReaderMapper/).

It can be downloaded by running this command on your own package manager console
```
dotnet add package Helpers.DataReaderMapper
```


# How to?
1- If you need to implement your own mapping from IDataReader object to your desired type, you can do it by calling the following method
```
string commandText = "Select * from dbo.TestTable";
var mappedList = new DatabaseConnector(sqlConnection).ReadDataFromSqlCommand(commandText, (dataReader) => YourMapMethod(dataReader));
```

2- If you need to use the implemented Mapping methodology, you can simply do it by annotating your desired mapped class properties with attribute ```Column["DatabaseColumnName"]``` then call the following method
```
string commandText = "Select * from dbo.TestTable";
var mappedList = new DatabaseConnector(sqlConnection).ReadDataFromSqlCommand(commandText);
```

