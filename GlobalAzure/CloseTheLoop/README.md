# Hands on Labs Global Azure Bootcamp 2017
## Vianen, April 22, 2017

## Azure Machine LEarning
Each Machine Learning experiment can be exposed as a webservice and embedded in a complete solution.

### 1. WebService
Azure ML studio can create a webservice for us.

1. Open the experiment
2. Make sure that the experiment is saved and has a running state (all items should have a green checkmark and in the top right of the canvas it shouls state: 'Finished nRunning')
3. Hover above 'Set up web service' in the bottom menu bar
4. Click 'Predictive Web Service [Recommended]'
5. Now you get a warning that you have to select a 'Train Model' item as this experiment has multiple
6. Select the 'Train Model' from the classification model (the bottom one, e.g. the last in the data flow)
7. Repeat the step above to create a webservice

After this step a new tab is created: 'Predictive experiment', some items are combined to a new item of the type 'Trained Model' and a webservice input and output are added to the dataflow. These are the entrypoints of the webservice that we can use to send and recieve data.
Before we can use it, we are deleting the clustering model part

8. Select and delet the folowwing items in the 'Predictive experiment':
    - Select Columns in Dataset (connected to first 'Edit Metadata')
    - K-Means Clustering
    - Train Clustering Model
    - Edit Metadata (connected with the 'Train Clustering Model' item)
    - Apply SQL Transformation
    - Join Data
9. Now we need to connect the 'Edit Metadata' output to the input of the 'Select Columns in Dataset' input
10. Next we need to delete the 'Assignment' column reference in the 'Select Columns in Dataset' items as this item is not known in the dataset
11. Save and run the experiment to update all the metadata
12. Add an extra 'Select Columns in Dataset' between 'Score Model' and 'Web service output' and connect the input and outputs correctly
13. Select only the `Scored Labels` and `Scored Probabilities` column
14. Now save the experiment and run the experiment to make sure that the webservice is working

After the experiment has runned succesfully it is possible to deploy the webservice
15. Click on the 'Deploy Web Service' in the bottom menu bar
16. Choose 'Deploy Web Service [Classic]'
17. A new window opens with the details of the webservice, including an API key and some test URLs

### 2. Test via Excel
Now that the webservice is created we can test it via Excel

1. Click on the 'Excel 2013 or later workbook' link next to the 'Batch Execution' 
2. You will get a warning about 'Sample Data' that the Excel workbook needs. If the experiment contains sensitive data this option should be disabled.
3. Open the downloaded Excel file and click 'Enable Content' to realy open the Excel file
4. A blank Excel sheet is visible with a 'Azure Machine Learning' pane at the right side. This is part of the AzureML Excel add-in and provides the option to interact with an AzureML webservice.
5. Click on the '\<Name>' of the webservice to connect to it
6. First create some sample data by clicking on the 'Use sample data' button.
7. Select the sample table (`Sheet1!$A$1:$K$6`) and use that as input; leave the 'My data has headers' checked
8. Set the Output to `L1` and leave the 'Include headers' checked
9. Press 'predict' and wait at the results
10. Depending on the sample results you will see that the 'Scored Label' is 0 or 1
11. You can change the values in the table and predict the new values by pressing 'Predict' again. This will overwrite the current values

## Azure Stream Analytics
Now we have a webservice that we can use in our Azure Stream Analytics query

### 3. Add user defined function (UDF)
First step is to add the webservice as User Defined Function to Stream Aanlytics

1. Open the ASA blade in the Azure portal (https://portal.azure.com/)
2. Click in the left list in the 'Job Topology' on 'Function'
3. Click 'Add'
4. Define a friendly function name, like 'FaultedSensor'
5. Choose 'AzureML' as function type
6. Choose 'Select from the same subscription'
7. Choose the newly created webservice
8. Paste the API key, which can be found on the details dashboard of the AzureML webservice
9. Press 'Create' to finish the UDF function

Now that the UDF is created we can tested the function and evaluate the outcome.

### 4. Alter ASA job query
After testing the UDF we can use it in our ASA query. 
One pro tip: The sensor data provides the location as a comma seperated string and the data we used in the Azure ML experiment was splited (by the USQL query). So we need to seprate that string in the ASA query before we can pass it to the AzureML UDF.

---
[Back](../README.md) | [License](../LICENSE)