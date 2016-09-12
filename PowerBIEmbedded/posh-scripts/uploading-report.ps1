# Define collection variable from https://portal.azure.com
$collection = '<collection>'

# Get an overview of the configures workspaces
powerbi get-workspaces -c $collection

# Set workspace variable from previous step
$workspace = '<workspaceId>'

# Get all created datasetsin a workspace
powerbi get-reports -c $collection -w $workspace

# Set variable with path to PBIX file
$PBIXfile = '<pathToPBIXFile>'

# Upload new report (Or update existing one)
powerbi import -c $collection -w $workspace -f $PBIXfile

$report = '<reportID>'
# Create Power Bi Embedded token, needed by the web application
powerbi create-embed-token -c $collection -w $workspace -r $report
