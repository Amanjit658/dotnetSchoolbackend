param(
    [string]$BaseUrl = 'http://localhost:5150'
)

# Only bypass certificate validation if using HTTPS for local testing
if ($BaseUrl -like 'https*') {
    [Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }
}

try {
    Write-Host "Calling /api/Auth/login at $BaseUrl..."
    $loginUri = "$BaseUrl/api/Auth/login"
    $login = Invoke-RestMethod -Uri $loginUri -Method Post -ContentType 'application/json' -Body (@{email='admin@school.com'; password='Admin@123'} | ConvertTo-Json)
    Write-Host "Login response:`n" ($login | ConvertTo-Json)
    $token = $login.token
    Write-Host "TOKEN: $token"

    Write-Host "Calling /api/Admin/create-teacher at $BaseUrl..."
    $createUri = "$BaseUrl/api/Admin/create-teacher"
    $body = @{ email = 'teacher2@mail.com'; password = 'Test@1234'; subject = 'Hindi'; fullName = 'Teacher 2' } | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri $createUri -Method Post -ContentType 'application/json' -Headers @{ Authorization = "Bearer $token" } -Body $body -ErrorAction Stop
    Write-Host "Create response:`n" ($resp | ConvertTo-Json)
}
catch {
    Write-Host "ERROR:`n" $_.Exception.ToString()
}
