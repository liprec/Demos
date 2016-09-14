#Define variables
$endpoint = "https://api.powerbi.com/beta/18..7e/datasets/85..89/rows?key=3W..3D"

# Import Car data from JSON
$carPoints = Get-Content -Path .\CarDemo.json | ConvertFrom-Json

$pushJson = {}.Invoke();
$newTime =  (Get-Date).AddTicks((New-TimeSpan (Get-Date $carPoints[0].Time) (Get-Date '1900/01/01T00:00:00')).Ticks);

for($i = 0; $i -le $carPoints.Length; $i++) {
    
    $carPoint = $carPoints[$i];
    $carTime = (Get-Date $carPoints[$i].Time);
    $carPoint.Time = $newTime;

    $pushJson.Add($carPoint);

    if ($carTime -ne (Get-Date $carPoints[$i+1].Time)) {
        
        $response = Invoke-RestMethod -Method Post -Uri "$endpoint" -Body (ConvertTo-Json @($pushJson))
        "Pushed " + $pushJson.Count + " data points to Power BI"

        $pushJson = {}.Invoke();
        $newTime =  (Get-Date).AddTicks((New-TimeSpan (Get-Date $carPoints[$i+1].Time) (Get-Date '1900/01/01T00:00:00')).Ticks);

        $wait = (New-TimeSpan (Get-Date $carPoints[$i+1].Time) (Get-Date $carPoints[$i].Time)).Milliseconds;

        "Thread waited for " + $wait + " miliseconds";

        Start-Sleep -m $wait
    }
}