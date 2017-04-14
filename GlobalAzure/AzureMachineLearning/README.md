# Hands on Labs Global Azure Bootcamp 2017
## Vianen, April 22, 2017

## Azure Machine Learning

If needed create an Azure Machine Learning resource in the Azure Portal.
1. Press the '+' sign on the top left
2. Search for 'Machine Learning Workspace'
3. Create using default / own values
4. After the creating is finished open the blade to continue with the next steps
5. Click on the link 'Machine Learning Workspace' and sign in with the same credentials

Alternative you can navigate to https://studio.azureml.net and create a free account.

Once signed in you enter the Microsoft Azure Machine Learning Studio. On the left you see:
- *Projects*: Here you can define projects to bundle different 
- *Experiments*: This is the main workspace where all the machine learning takes part
- *Web Services*: An AzureML model can be exposed via a webservice and via that way used as part of a different solution
- *Notebooks*: A nootbook is an interactive enviroment () to experiment with data and models
- *Datasets*: What is machine learning without data: this is were all locally stored datasets can be managed
- *Trained Models*: An typical end result of a machine learning solution is a trained model. And that model can be used in a different experiment. This is the place where those models are managed.
- *Settings*: Of course you can alter some settings for Microsoft Azure Machine Learning Studio

### Introduction in Machine Learning

In our scenario we have +/- 1000 temperature and humidity sensors that generates messages. We want to determine if there our faulted measures/sensors and we suspect that the temperature measure is faulting. To test this hypothesis we need to follow as all Machine Learning projects these five steps:

1. Data selection.
2. Preprocessing.
3. Transformation.
4. Data Mining.
5. Interpretation and evaluation

Beware: it is an iterative process in which some, or all, steps may be repeated.

### Data extract

At this moment AzureML is not capable in directly importing data from an Azure Data Lake Storage, so first download the output of the last excersise of the previous lab and save it to disk. And to import the data to Azure ML studio:

1. Click on '+ New' on the left bottom and select 'Dataset' -> 'From Local File'
2. Click on 'Browse' and navigate to the downloaded file
3. Enter a friendly name
4. Choose as 'Generic CSV File With no header (.nh.csv)'
5. Press the checkmark to start the upload of the file  

If you didn't finish the previous lab and/or you don't have a correct input file, follow the last portion of thye steps in the next paragraph.

### Data selection

First step is getting the data into a new experiment.

1. Click on '+ New' on the left bottom and select 'Blank Experiment'
2. The default title is 'Experiment created on \<date>', but can easily changed to a logical name
3. The left toolbox has all the items that can be used in a experiment
4. Click in the search box and search for the name you provided
5. Select and drag the found dataset to the canvas

You can continue to the next paragraph as the next steps describes the steps if no own file is available.
If you didn't finish the previous lab and/or you don't have a correct input file, follow these steps:

1. Search for 'Import Data'
2. Drag the item to the canvas
3. Configure it with:
    - Data Source: Web URL with HTTP
    - Data Source URL: https://globalazurecis.blob.core.windows.net/adls/08.demo.out
    - Keep the rest default
4. Continue with the rest of the steps

### Preparation

Now that we have imported a dataset we can examen it. Users that are using the import data items: click on the Play button to download the file.
1. Right click on the output of the item
2. In the context menu select 'Visualize'
3. A popup window is visible with some basic information of the data set:
    - On the top you see how many rows and column the dataset has
    - In the table the first 200 rows are visible
    - Just below the header of the table you see a graphical representation of the column data
    - If a column is selected basic statistical information is shown on the right. For numeric columns more statistical information is available.
4. In this view we can see that there are no column headers, so every column is called: 'col\<number>'

Now we are going to change the header so it easier to work with the data.
1. Search for the 'Edit Metadata' item
2. Connect the data output with th input of 'Edit Metadata' item
3. Click on the 'Launch column selector' to lanch the column selector window
4. In this window you can select column in different ways:
    - By Name: this option is only available if the item has runned (little green checkmark to the right of the text) and the columnnames are known
    - With Rules: with this option you can create in different ways a column selection. First slection is the starting rule: with or without column, so if we are excluding or including columns. After that we can add extra rules to either include or exclude columns by varios rules.
5. Here we are using the 'With rules' option and we start with all columns as we want to rename all column names
6. After closing the column selector window paste the following comma seperated string in the 'New column names' textbox
    `time,dsplid,dspl,temp,hmdt,status,building,floor,EventProcessedUtcTime,PartitionId,EventEnqueuedUtcTime`
7. Run the experiment and take a look al the 'Visualize' windowto validate that the column names are changed

### Transformation
The operational department has mentioned that there are sensors messages that are not valid and should be ignored. And the way to identify them: there 'status' column is 'O.' So we are going to filter the data

1. Search for the 'Split Data' item
2. As mode select 'Regular Expression'
3. Use `\"status" ^OK` as expression
4. Run the experiment and validate that there are only status 'OK' in the 1st output

Before we start with the next part we need to validate if the hypothesis is valid, so we are taking a closer look to the data

1. Search for the 'Apply SQL Transformation'
2. Connect the 1st output of the 'Split Data' to the 1st input
3. Paste the following SQL query in the 'SQL Query Script'
```
select distinct
    temp,
    count(*) cnt
from t1;
```
4. Run the experiment and validate that our hypothesis is valid
5. For the next phase we can delete this item

Next step is to start create a model and automaticly identify faulted sensors

### Data Mining
Now that we have out data prepared and in the correct format we can start with the data mining part. There are two types of algoritms:
- Supervised learning: These algoritm needs to be trained and therefor we know the correct outcome at the start. And we are using related information to create a model that predicts the outcome as close as possible. Typical scenario: we start with a list of all the sensors and a label if it is working yes or no, And we are calculating what attrubites of that sensor are predicting if it is working. Here we can use a clustering algoritm.
- Unsupervised learning: These algoritm doesn;t need to be trained and uses the whole dataset to create a model. A typical scenario: a list of sensors and there tempuratures and we need to determine which values of which sensor are outliers. This is a case we use a classification algoritm.

At this moment we do not yet know which sensors are faulted, so we are going to use a clustering to determine which temperature values are faulted. And after that we are using a classification the determine if there are other factors that can predict a faulted sensor.

First the clustering:
1. First select only the `dsplid` and `temp` values with the 'Select Columns in Dataset' item from the dataset (output 'Split Data')
2. Add a 'K-Means Clustering' item to the canvas and as it only has an output it cannot be connected to another output
3. Add a 'Train Clustering Model' and connect both the 'K-Means Clustering' and the 'Select Columns in Dataset' to the correct inputs
4. Select `temp` as the values to be used for the clustering algoritm
5. Add an 'Edit Metadata' and connect it to the output of the 'Train Clustering Model' item
6. Select `Assignments` as input column and use 'Make Categorial' as option for 'Categorial' option
7. Add an 'Apply SQL Transformation' and use the following query to output a list of faulted (1) sensors
```
select
    dsplid,
    max(Assignments) Assignments
from t1
group by dsplid;
```
8. Run the experiment and validate if there are any sensors that are faulted

![](images/Clustering.png)

### Interpretation and evaluation

---
[Back](../README.md)