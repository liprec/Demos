# Hands on Labs Global Azure Bootcamp 2017
## Vianen, April 22, 2017

## Azure Stream Analytics

If needed create an Azure Stream Analytics resource in the Azure Portal.
1. Press the '+' sign on the top left
2. Search for 'Stream Analytics job'
3. Create using default / own values
4. After the creating is finished open the blade to continue with the next steps

### 1. Create Input

Next step is to create an input for the Azure Stream Analytics job.
1. In the 'Overview' of the Stream Analytics job blade click on 'Inputs'
2. Click on '+ Add'
3. Provide a logical input alias. This name is used later in the query
4. Make sure that the type is 'Data Stream' and source is 'Event hub'
5. Choose 'Provide event hub settings manually'
6. Fill the remaining values with the values from the Excel sheet.
7. Make sure that the 'Event hub consumer group' is set to the correct value to minimize the interferance with the others members
8. Keep the default format and enconding
9. Click on 'Create' to finilize the input definition

### 2. Sample Input

Now that the input is created we can download some sample data that we need when testing the Stream Analytics queries

1. Click on the newly created input
2. Click on the 'Sample Data' on the top menu of the blade
3. A new blade is opening and prefilled with the default date and time
4. Alter the start time and the duration to gather the events of the last hour.
5. After clicking 'OK' a background task is triggered
6. After that background task is finished you will get a natification
7. Open that notification and download the sample JSON to disk.

With these steps we have created a connection to an event hub that is providing temperature sensor data and have this datastream available as input.

### 3. Create Power BI Output

After we created an input for the ASA job, an output is also needed.

1. In the 'Overview' of the Stream Analytics job blade click on 'Outputs'
2. Click on '+ Add'
3. Provide a logical output alias
4. Make sure that the 'Sink' is set to 'Power BI'
5. Click on authorize and provide login details (work email!) and additional authorizations
6. After a succesful authorization provide the additional detials, like workspace, dataset and table name
7. Finish by clicking the 'Create' button 

After the output is created we have a direct output to Power BI and the capability to stream data to a table in a dataset. You can take a look in the Power BI portal, but the dataset and table are not yet created. Those will be created when the job is started.

### 4. Create ASA Job

With an input and an output we can connect those with a query.

1. In the 'Overview' of the Stream Analytics job blade click on 'Query'
2. On the left we have the aliases of both the input and the output. Also the query editor has already a default query that directly connects the input to the output
3. Review the query and see how familiar you are with the query language
4. Press 'Save' if needed to save the query
5. You can test the query by pressing 'Test' and upload the provided JSON file to the input
6. Review the test result

Now we have a default connection between the input and output, but that output is not that valuable for Power BI. The next steps are needed to alter the input to be useful for Power BI.
7. Alter the query to add some extra logic. See included file `01 PowerBI - Select query.saql`
8. Review the query and understand what the query is achieving
9. Save the query and test this query by uploading the providede test file
10. Review the test results and close the query blade

After creating the job we can start it by pressing the 'Start' button. It takes some time to allocate resources and to really start.

### 5. Create Power BI dashboard

In the meantime we can start creating a Power BI dashboard.
1. Login to the Power BI portal via https://powerbi.com
2. Navigate to the DataSets. Make sure you navigate to the same workspace as used when creating the output
3. Click at 'Create Report' under actions to create a new report
4. Create a visual, eq line chart, and use a value as 'Values' and the time as 'Axis'
5. Save the report and pin the visual to a new dashboard
6. Open the dashboard and observe

### 6. Add windowing

A typical scenario with streaming data is that we are interested in (average) values during a period of time. Usually one single spike in the observations is not really a problem, but if it us recurring it will be. With Stream Analytics we can create a window and push the values of that window to the output.

1. Look at the window functionality at: https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-window-functions
2. Now create a query that creates a window of 1 second and returns only the maximum of the 'temp' and 'hmdt' value
3. You can of course test your query by uploading the test sample

Now that we have looked at (some) capabilities of Azure Stream Analytics fele free to experiment and look at the result in Power BI.

### Reference
ASA query language: https://msdn.microsoft.com/library/azure/dn834998.aspx

---
[Back](../README.md) | [License](../LICENSE)