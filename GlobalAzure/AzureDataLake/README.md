# Hands on Labs Global Azure Bootcamp 2017
## Vianen, April 22, 2017

## Azure Data Lake Store
All the files are stored within the Azure Data Lake in the Azure Data Lake Store (ADLS) (or can be connected directly to ADLS, like an on-prem HDFS cluster).

If needed create an Azure Data Lake Store resource in the Azure Portal.
1. Press the '+' sign on the top left
2. Search for 'Azure Data Lake Store'
3. Create using default / own values
4. After the creating is finished open the blade to continue with the next steps

### 1. Adding extra ASA output
To continue this integrated lab we need to alter our Azure Stream Analytics job to store all JSON data to an ADLS store.

First create a new output to the newly created ADLS:
1. Open the Azure Stream Analytics blade used in the previous lab
2. Add an extra output and choose 'Data Lake' as sink
3. 'Authorize' the Azure Stream Analytics access to the Azure Data Lake
4. Create a logic 'Path prefix pattern'
5. Make sure that 'Line Seperated' is selected as format
6. Save/Create the new output

### 2. Alter Azure Stream Analytics job
Now alter the Azure Stream Analytics job to store all eventhub directly to the Azure Data Lake Store.
If everything works as designed a folder structure and JSON file(s) will be saved to the ADLS.

## Azure Data Lake Analytics
The power of Azure Data Lake extract information from stored (text) files within the ADLS via Azure Data Lake Analytics (ADLA) by using U-SQL queries.

If needed create an Azure Data Lake Analytics resource in the Azure Portal.
1. Press the '+' sign on the top left
2. Search for 'Azure Data Lake Analytics'
3. Create using default / own values
4. Make sure to link the previous created ADLS
5. After the creating is finished open the blade to continue with the next steps

### 1. Basic U-SQL query (local)
The first U-SQL query we are creating is a query to extract rows from all the stored files. Developing and testing of U-SQL queries is done via Visual Studio, so we need to download a sample JSON file from the store and store it in the correct local path to emulate the ADLS version.

1. Navigate in the Azure Data Lake Store Data Explorer to a JSON file and click download
2. Open Visual Studio (2017/2015 with the Azure Data Lake Tools installed) and create a new 'U-SQL Project'
3. By default a U-SQL project create a project with two files: a `script.usql` file and a 'code-behind' `script.usql.cs` file. With that last file additional C# code can be added to the ADLA job for extra additional operations.
4. Because ADLA job use data stored in Azure Data Lake Storage, we need to store the downloaded file in the correct local equivalent.
5. Default is 'C:\Users\<UserName>\AppData\Local\USQLDataRoot', but it can be changed to a custom folder via the 'Data Lake' -> 'Options and settings'
6. Copy the file to the path above with the same 'Path prefix pattern' as configured in the previous steps to mimic the ADLS

Now that the store is correctly emulated we can start creating our first U-SQL query
7. Open the `01. Script.usql` script file and copy the content to your own version
8. Alter the 'start variables' to the correct values
9. An U-SQL query usally has three parts: 1) extract 2) transform 3) store output. In this query the transform part is eliminated for simplicity
10. Look at the query and see the following typical parts of an U-SQL query:
    - UPPER case syntax is related to SQL
    - CamelCasing syntax is related to C# code
11. In this query we are using build-in 'Extractors' and 'Outputters' to extract the individual rows and store the output as CSV file
12. Run the job by clicking 'Submit'
13. Evaluate the output file which can be found in the output folder of the local storage

### 2. Submit to ADLA
Now that we have a working U-SQL job we can easily submit this query to Azure Data Lake Analytics and run it against our stored data in Azure Data Lake Store

First we need Visual Studio to know that there is an Azure Data Lake Analytics account that he can 
1. Open the Cloud Explorer (View -> Cloud Explorer)
2. Sign In with the correct account
3. Find in the tree the created Azure Data Lake Analytics
4. Now that Visual Studio knows the Azure Data Lake Analytics account it is possible to submit the job to Azure
5. Next to the '(local)' you can choose the Azure Data Lake Analytics account. (Reopen the usql file can help)
6. Via the 'Advanced' options of 'Submit' we can alter the parameters of the job
7. At this moment there is no reason to alter the default, but it is possible to alter the parallelism value and also the priority of the job
8. By pressing 'Submit' the job is runned in Azure and against the Azure Data Lake Store files

### 3. Job output
After the job has been submitted a new window is opened and showing the steps of the job.

First the job is beeing prepared: job is compiled and prepared to be scheduled
Next the job is queued and resources are allocated and after that the real execution of the job starts.
The 'Job Graph' provides a graphical overview of the steps the job had taken during execution. The number of vertexes gives some indication of the number of parallelism.
And the last step is the finalizing step which is mainly de-allocating resources and writing all the logging information.

This window can also be viewed online and there is also an estimate of the costs.

![](images/ADLA-JobOutcome.png)

### 4. Expanding input
Now that we have run a job versus one file, an extra dimension of this scenario is that we can run the same query against a dynamic set of files.

With the syntax `{date:yyyy}`, `{date:MM}`, `{date:dd}` and `{date:hh}` we can create the same 'Path prefix pattern' as we used in the output of the Azure Stream Analytics so the job will use all the files we have collected via ASA of our sensors.

1. Open or create a copy of the previous query file (`01. Script.usql`)
2. Alter the `@INPUT_PATH` variable with the above syntax elements so the job will use all files stored in ADLS
3. Add the required virtual column to the 'Extract' part of the query
4. First test the job by running it locally. The output should almost be the same
5. Submit the job to ADLA and look what has changed in the Job Graph

### 5. Parsing JSON
Now that we understand the basic logic of a U-SQL query and job we will start with adding a transform part to the query
For easily parsing JSON data we are going to use precompiled custom code. This code can be used after the required assemblies are registered.

1. Open the Cloud Explorer in Visual Studio
2. Open the Azure Data Lake Analytics and navigate to the Databases node
3. Open the 'Assemblies' node in the 'master' database
4. Right click 'Register Assembly' and use the two assemblies in the 'Include' folder
5. Be sure to register the assemblies in both the (local) and ADLA master database 

After both tghe assemblies are registered in the master databse we can reference them in a U-SQL query 

6. Open or create a copy of the  previous query files which uses the single file query (`01. Script.usql`) file
7. Add two references to the newly registered assemblies as the first lines of the query file
```
REFERENCE ASSEMBLY [Newtonsoft.Json];
REFERENCE ASSEMBLY [Microsoft.Analytics.Samples.Formats];
```
8. Add a first transformation to parse the JSON from the extractor. Hint: `Microsoft.Analytics.Samples.Formats.Json.JsonFunctions.JsonTuple()`
9. Add an second transformation to return the individual dictonary returned by the `JsonTuple` method as columns
10. Output all the columns as a CSV file

### 6. Extracting extra column information
Now that we have the individual columns of our sensor data, we still need to parse some columns to either convert them to the correct datatypes (like dates) or split the provided information.

First step is to parse the dates to a correct datatype.

1. Open or create a copy of the previous query file (`03.demo.usql`)
2. Convert all the columns containing date and/or time values with `Convert.ToDateTime()`
3. Do the same for all the columns containing numbers
4. Test the result by submitting the query as a job

The column 'Location' contains the location of the sensor and the building and floor is seperated by a comma. To exstract this information we are creating a custom method in the code behind file. Ofcourse it is possible  to create and register a general assembly for this purpose.

5. Open the code behind file
6. Create a `public class`
7. Create a method (`public string GetBuilding()`)
8. Return the first part of the `split` of the string (Hint: `string.Split(',')`)
9. Alter the query to use the custom method for location
10. Test the result by submitting the query as a job

You can now do the same with the floor. Technical it is not the best way to create two methods to split a string and return one of the two parts. Best way is to return a `SqlMap` object as a dictionary and use that object to get individual values. Same as the `JsonParser` os working, but is left out of scope for this lab.

### 7. Filtering data
Now that we have all the columns in the correct values, we can add extra filtering options to the query to get a correct dataset we can use for the next lab: Azure Machine Learning

Now we are filtering all the sensors that have an empty floor as we cannot locate those sensors. You can use regular SQL WHERE statements for this. Only the not equal is different (!=) as U-SQL has a lot of C# references.

Also make sure that this last query is submitted to ADLA against all the JSON files.

After this lab we have an output file that we are going to use for the next lab.

### Reference
See https://github.com/Azure/usql/tree/master/Examples for more examples

---
[Back](../README.md) | [License](../LICENSE)