<#
.SYNOPSIS
  Hook SessionStart : met à jour claude-config et signale les fichiers copiés dans ce projet qui sont périmés.
.NOTES
  Copié dans <projet>/.claude/hooks par sync.ps1 et déclaré dans .claude/settings.json. Ne modifie jamais le projet.
  claude-config est cherché dans $env:CLAUDE_CONFIG_DIR, sans autre emplacement par défaut.
  Sans effet quand la variable n'est pas définie ou que claude-config est absent (cloud, autre poste). Silencieux quand tout est à jour ; la sortie standard est ajoutée au contexte de la session.
#>
$ErrorActionPreference = 'Continue'
$project = if ($env:CLAUDE_PROJECT_DIR) { $env:CLAUDE_PROJECT_DIR } else { (Get-Location).Path }
if (-not (Test-Path (Join-Path $project '.claude/claude-config.lock.json'))) { exit 0 }

$repo = $env:CLAUDE_CONFIG_DIR
if (-not $repo -or -not (Test-Path (Join-Path $repo 'sync.ps1'))) { exit 0 }

$fetch = Start-Job { param($r) git -C $r fetch --quiet 2>$null; $LASTEXITCODE } -ArgumentList $repo
$finished = Wait-Job $fetch -Timeout 8
$fetchOk = $finished -and ((Receive-Job $fetch) -contains 0)
Remove-Job $fetch -Force
if ($fetchOk) { git -C $repo merge --ff-only --quiet '@{u}' 2>$null | Out-Null }
if (-not $fetchOk -or $LASTEXITCODE -ne 0) {
    Write-Output 'claude-config : mise à jour impossible (hors ligne, changements locaux ou divergence). Les fichiers copiés peuvent être périmés.'
}

& (Join-Path $repo 'sync.ps1') $project -Check
exit 0
