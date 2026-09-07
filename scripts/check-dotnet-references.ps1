$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$projectPaths = [ordered]@{
    "services/api/src/CoffeePos.Domain/CoffeePos.Domain.csproj" = @()
    "services/api/src/CoffeePos.Application/CoffeePos.Application.csproj" = @(
        "services/api/src/CoffeePos.Domain/CoffeePos.Domain.csproj"
    )
    "services/api/src/CoffeePos.Infrastructure/CoffeePos.Infrastructure.csproj" = @(
        "services/api/src/CoffeePos.Application/CoffeePos.Application.csproj",
        "services/api/src/CoffeePos.Domain/CoffeePos.Domain.csproj"
    )
    "services/api/src/CoffeePos.ApiHost/CoffeePos.ApiHost.csproj" = @(
        "services/api/src/CoffeePos.Application/CoffeePos.Application.csproj",
        "services/api/src/CoffeePos.Infrastructure/CoffeePos.Infrastructure.csproj"
    )
    "services/worker/src/CoffeePos.Worker/CoffeePos.Worker.csproj" = @(
        "services/api/src/CoffeePos.Application/CoffeePos.Application.csproj",
        "services/api/src/CoffeePos.Infrastructure/CoffeePos.Infrastructure.csproj"
    )
}

function Normalize-RelativePath([string]$path) {
    return $path.Replace([IO.Path]::DirectorySeparatorChar, "/").Replace([IO.Path]::AltDirectorySeparatorChar, "/")
}

function Get-ProjectReferences([string]$projectPath) {
    [xml]$document = Get-Content -LiteralPath $projectPath -Raw
    $projectDirectory = Split-Path -Parent $projectPath
    $references = @(
        $document.Project.ItemGroup.ProjectReference | Where-Object { $_ -and $_.Include } | ForEach-Object {
            $included = [string]$_.Include
            $resolved = [IO.Path]::GetFullPath((Join-Path $projectDirectory $included))
            Normalize-RelativePath ([IO.Path]::GetRelativePath($root, $resolved))
        }
    )
    return @($references | Sort-Object)
}

foreach ($relativeProject in $projectPaths.Keys) {
    $projectPath = Join-Path $root $relativeProject
    if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
        throw "Missing project file: $relativeProject"
    }

    [xml]$document = Get-Content -LiteralPath $projectPath -Raw
    $targetFramework = [string]$document.Project.PropertyGroup.TargetFramework
    if ($targetFramework -ne "net10.0") {
        throw "$relativeProject must target net10.0; found '$targetFramework'"
    }

    $actual = @(Get-ProjectReferences $projectPath)
    $expected = @($projectPaths[$relativeProject] | Sort-Object)
    $difference = @()
    if ($expected.Count -gt 0 -or $actual.Count -gt 0) {
        $difference = @(Compare-Object -ReferenceObject $expected -DifferenceObject $actual)
    }
    if ($difference.Count -gt 0) {
        throw "Project-reference graph mismatch in ${relativeProject}: $($difference | Out-String)"
    }
}

foreach ($relativeProject in @(
    "services/api/src/CoffeePos.Domain/CoffeePos.Domain.csproj",
    "services/api/src/CoffeePos.Application/CoffeePos.Application.csproj"
)) {
    $projectPath = Join-Path $root $relativeProject
    $content = Get-Content -LiteralPath $projectPath -Raw
    if ($content -match "Microsoft\.AspNetCore|EntityFrameworkCore|Npgsql|StackExchange\.Redis|PostgreSQL|Redis") {
        throw "$relativeProject contains a forbidden framework or infrastructure dependency"
    }
}

Write-Output "Project-reference checks passed: exact graph and Domain/Application dependency boundaries are valid."
