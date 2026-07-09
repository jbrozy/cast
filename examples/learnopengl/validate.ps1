# validate-shaders.ps1

$Validator = "glslangValidator.exe"
# oder:
# $Validator = "glslValidator.exe"

$ErrorCount = 0
$OkCount = 0

# Nur direkte Ordner wie ./advanced_lighting_build, ./shadow_mapping_build usw.
$BuildDirs = Get-ChildItem -Path ".\*_build" -Directory

if (-not $BuildDirs) {
    Write-Host "Keine direkten *_build-Ordner gefunden."
    exit 0
}

$ShaderFiles = @()

foreach ($Dir in $BuildDirs) {
    $ShaderFiles += Get-ChildItem -Path $Dir.FullName -File -Filter "*.vsh"
    $ShaderFiles += Get-ChildItem -Path $Dir.FullName -File -Filter "*.fsh"
}

if (-not $ShaderFiles) {
    Write-Host "Keine .vsh oder .fsh Dateien in direkten *_build-Ordnern gefunden."
    exit 0
}

foreach ($File in $ShaderFiles) {
    switch ($File.Extension.ToLower()) {
        ".vsh" { $Stage = "vert" }
        ".fsh" { $Stage = "frag" }
        default { continue }
    }

    Write-Host ""
    Write-Host "Pruefe: $($File.FullName)"
    Write-Host "Stage : $Stage"

    & $Validator -S $Stage $File.FullName

    if ($LASTEXITCODE -eq 0) {
        Write-Host "OK: $($File.FullName)" -ForegroundColor Green
        $OkCount++
    } else {
        Write-Host "FEHLER: $($File.FullName)" -ForegroundColor Red
        $ErrorCount++
    }
}

Write-Host ""
Write-Host "=============================="
Write-Host "GLSL Validation abgeschlossen"
Write-Host "OK     : $OkCount"
Write-Host "Fehler : $ErrorCount"
Write-Host "=============================="

if ($ErrorCount -gt 0) {
    exit 1
}

exit 0
