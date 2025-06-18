# smoke-test.ps1

# 1. Parametry
$apiBase = "https://localhost:7098/api"    # dostosuj port jeśli inny
$user     = "admin@yourdomain.com"         # istniejący użytkownik Admin
$pass     = "YourPassword123!"

# 2. Logowanie → pobranie JWT
Write-Host "Logging in..."
$loginBody = @{ Email = $user; Password = $pass } | ConvertTo-Json
$loginRes  = Invoke-RestMethod -Uri "$apiBase/Auth/login" `
    -Method Post -Body $loginBody -ContentType "application/json"
$token     = $loginRes.Token
if (-not $token) { throw "Login failed!" }
Write-Host "Token:" $token.Substring(0,20) "..."; "`n"

# Helper do nagłówków
$headers = @{ Authorization = "Bearer $token" }

# 3. CRUD dla Customer
Write-Host "Creating Customer..."
$newCust = @{ FirstName = "Test"; LastName = "User"; Email = "test.user@x.com" } | ConvertTo-Json
$custRes = Invoke-RestMethod -Uri "$apiBase/Customer" -Method Post `
           -Body $newCust -ContentType "application/json" -Headers $headers
$custId  = $custRes.Id
Write-Host "Created Customer Id =" $custId; "`n"

Write-Host "Getting all Customers..."
Invoke-RestMethod -Uri "$apiBase/Customer" -Headers $headers | Format-Table

Write-Host "Getting Customer by Id..."
Invoke-RestMethod -Uri "$apiBase/Customer/$custId" -Headers $headers | Format-List

Write-Host "Deleting Customer..."
Invoke-RestMethod -Uri "$apiBase/Customer/$custId" -Method Delete -Headers $headers
Write-Host "Deleted Customer $custId"; "`n"

# 4. CRUD dla Rental
Write-Host "Creating Car..."
$newCar = @{ Make="Toyota"; Model="Corolla"; Year=2020; Brand="Toyota" } | ConvertTo-Json
$carRes = Invoke-RestMethod -Uri "$apiBase/Car" -Method Post `
         -Body $newCar -ContentType "application/json" -Headers $headers
$carId  = $carRes.Id
Write-Host "Created Car Id =" $carId; "`n"

Write-Host "Creating Rental..."
$newRental = @{ CarId=$carId; CustomerId=$custId; RentalDate=(Get-Date).ToString("o") } | ConvertTo-Json
$rentalRes = Invoke-RestMethod -Uri "$apiBase/Rental" -Method Post `
             -Body $newRental -ContentType "application/json" -Headers $headers
$rentalId = $rentalRes.Id
Write-Host "Created Rental Id =" $rentalId; "`n"

Write-Host "Getting all Rentals..."
Invoke-RestMethod -Uri "$apiBase/Rental" -Headers $headers | Format-Table

Write-Host "Getting Rental by Id..."
Invoke-RestMethod -Uri "$apiBase/Rental/$rentalId" -Headers $headers | Format-List

Write-Host "Deleting Rental..."
Invoke-RestMethod -Uri "$apiBase/Rental/$rentalId" -Method Delete -Headers $headers
Write-Host "Deleted Rental $rentalId"; "`n"

Write-Host "Smoke test complete!"
