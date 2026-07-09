param(
    [string]$Validator = "glslangValidator.exe",
    [string]$Root = "."
)

$results = @()
$hasFailures = $false

$buildDirs = Get-ChildItem -Path $Root -Directory -Recurse -Filter "*_build" |
    Sort-Object FullName

foreach ($dir in $buildDirs) {
    $name = $dir.Name -replace "_build$", ""

    $shaderFiles = @()
    $shaderFiles += Get-ChildItem -Path $dir.FullName -File -Filter "*.vsh" -ErrorAction SilentlyContinue
    $shaderFiles += Get-ChildItem -Path $dir.FullName -File -Filter "*.fsh" -ErrorAction SilentlyContinue

    $shaderFiles = $shaderFiles | Sort-Object Name

    $fileResults = @()
    $folderOk = $true

    if ($shaderFiles.Count -eq 0) {
        $folderOk = $false
        $hasFailures = $true

        $results += [pscustomobject]@{
            Name = $name
            Ok = $false
            Files = @()
            Message = "NO SHADERS"
        }

        continue
    }

    foreach ($file in $shaderFiles) {
        $stage = switch ($file.Extension.ToLower()) {
            ".vsh" { "vert" }
            ".fsh" { "frag" }
            default { $null }
        }

        if ($null -eq $stage) {
            continue
        }

        $output = & $Validator -S $stage $file.FullName 2>&1
        $exitCode = $LASTEXITCODE

        if ($exitCode -eq 0) {
            $fileResults += [pscustomobject]@{
                File = $file.Name
                Ok = $true
                Message = "OK"
                Output = $output
            }
        } else {
            $folderOk = $false
            $hasFailures = $true

            $fileResults += [pscustomobject]@{
                File = $file.Name
                Ok = $false
                Message = "FAILED"
                Output = $output
            }
        }
    }

    $results += [pscustomobject]@{
        Name = $name
        Ok = $folderOk
        Files = @($fileResults)
        Message = if ($folderOk) { "OK" } else { "FAILED" }
    }
}

Write-Host ""
Write-Host "Summary"
Write-Host "======="

foreach ($result in $results) {
    if ($result.Ok) {
        Write-Host "$($result.Name): OK" -ForegroundColor Green
    } else {
        Write-Host "$($result.Name): $($result.Message)" -ForegroundColor Red
    }

    foreach ($fileResult in $result.Files) {
        if ($fileResult.Ok) {
            Write-Host "  $($fileResult.File): OK" -ForegroundColor Green
        } else {
            Write-Host "  $($fileResult.File): FAILED" -ForegroundColor Red

            if ($fileResult.Output) {
                foreach ($line in $fileResult.Output) {
                    Write-Host "    $line" -ForegroundColor Red
                }
            }
        }
    }

    Write-Host ""
}

if ($hasFailures) {
    exit 1
}

exit 0
