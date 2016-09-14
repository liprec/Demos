# Power BI Steaming Data

Simple demo to demostrate the (new) Power BI Streaming datasets

# Power BI Streamning Dataset
Create a new streaming data set with the following columns and datatypes

- ID : Number
- Source : Text
- Latitude : Number
- Longitude : Number
- Speed : Number
- Altitude : Number
- Time : DateTime
- SourceID : Text 

# First steps
The created streaming dataset as a unique access url. Paste that url in the `PushDataToPowerBI.ps1`
as value of the `$endpoint`.

#  How to start the demos
Open the ps1 file with PowerShell (ISE or Visual Code) and run the program.
It will send data to the streaming dataset.

Based on this dataset realtime dashboards can be created.

## Reference
The car data is based on an experiment conducted at the University of Stuttgart ([missing link]())
and modified for this purpose.
 