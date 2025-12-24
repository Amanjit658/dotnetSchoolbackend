[Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

# start server detached on ports 5155/7255
Remove-Item -Path 'c:\temp\app_out.log' -ErrorAction SilentlyContinue
Remove-Item -Path 'c:\temp\app_err.log' -ErrorAction SilentlyContinue
$proj = 'c:\Users\hp\source\repos\myFirstSchoolProject\myFirstSchoolProject.csproj'
$proc = Start-Process -FilePath 'dotnet' -ArgumentList 'run','--no-build','--project',$proj,'--urls','http://localhost:5155;https://localhost:7255' -RedirectStandardOutput 'c:\temp\app_out.log' -RedirectStandardError 'c:\temp\app_err.log' -PassThru
Write-Host "Started process Id: $($proc.Id)"
Start-Sleep -Seconds 4
Write-Host '--- Server logs (tail) ---'
if (Test-Path 'c:\temp\app_out.log') { Get-Content -Path 'c:\temp\app_out.log' -Tail 200 } else { Write-Host 'No server log yet' }
Write-Host '--- Netstat ---'
netstat -ano | findstr 7255

# Attempt login and create-teacher
try {
    Write-Host '--- Trying login ---'
    $login = Invoke-RestMethod -Uri 'https://localhost:7255/api/Auth/login' -Method Post -ContentType 'application/json' -Body (@{email='admin@school.com'; password='Admin@123'} | ConvertTo-Json)
    Write-Host 'Login response:'
    $login | ConvertTo-Json
    $token = $login.token
    Write-Host "TOKEN: $token"

    Write-Host '--- Calling create-teacher ---'
    $body = @{ email = 'teacher_run@mail.com'; password = 'Test@1234'; subject = 'Hindi'; fullName = 'Teacher Run' } | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri 'https://localhost:7255/api/Admin/create-teacher' -Method Post -ContentType 'application/json' -Headers @{ Authorization = "Bearer $token" } -Body $body -ErrorAction Stop
    Write-Host 'Create response:'
    $resp | ConvertTo-Json
}
catch {
    Write-Host 'ERROR:'
    Write-Host $_.Exception.ToString()
    if ($_.Exception.Response -ne $null) {
        try {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = [System.IO.StreamReader]::new($stream)
            $text = $reader.ReadToEnd()
            Write-Host 'Response body:'
            Write-Host $text
        } catch {
            Write-Host 'Could not read response body.'
        }
    }
}

Write-Host '--- Final server log tail ---'
if (Test-Path 'c:\temp\app_out.log') { Get-Content -Path 'c:\temp\app_out.log' -Tail 200 }
