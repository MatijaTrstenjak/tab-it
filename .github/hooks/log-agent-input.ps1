param(
    [Parameter(Mandatory = $true)]
    [string]$EventName
)

$ErrorActionPreference = "Stop"

$logPath = "C:\Repo\tab-it\agent_log.txt"
$logDir = Split-Path -Parent $logPath

if (-not (Test-Path -Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

$payload = [Console]::In.ReadToEnd()

$entry = @{
    timestamp = (Get-Date).ToString("o")
    event = $EventName
    payload = $payload
}

$entryJson = $entry | ConvertTo-Json -Compress -Depth 8
Add-Content -Path $logPath -Value $entryJson

exit 0
