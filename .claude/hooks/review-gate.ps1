<#
.SYNOPSIS
  Garde-fou Claude Code : refuse `git commit` tant que /review-modified-files n'a pas été passé sur l'état courant du code.
.NOTES
  Sans argument : hook PreToolUse sur Bash (JSON sur stdin ; code 2 = appel refusé, stderr est renvoyé à Claude).
  -Mark : appelé par le skill review-modified-files en fin de revue ; enregistre l'empreinte de l'état courant.
  L'empreinte (git diff HEAD + fichiers non suivis) est stockée dans le dossier git, donc jamais commitée.
  Un commit qui ne touche pas de code (documentation, rules, skills) n'est pas bloqué.
#>
param([switch]$Mark)

$ErrorActionPreference = 'Continue'
$codePattern = '\.(cs|razor|cshtml|csproj|props|sql|ts|tsx|js|css|scss|html|htm|feature)$'

$start = if ($env:CLAUDE_PROJECT_DIR) { $env:CLAUDE_PROJECT_DIR } else { (Get-Location).Path }
$root = git -C $start rev-parse --show-toplevel 2>$null
if (-not $root) { exit 0 }

function Get-ChangedFiles {
    @(git -C $root -c core.quotepath=off diff HEAD --name-only) + @(git -C $root -c core.quotepath=off ls-files --others --exclude-standard) | Where-Object { $_ }
}

function Get-Fingerprint {
    $parts = @(git -C $root diff HEAD)
    foreach ($file in @(git -C $root -c core.quotepath=off ls-files --others --exclude-standard)) {
        $parts += "$file $(git -C $root hash-object -- $file)"
    }
    $bytes = [Text.Encoding]::UTF8.GetBytes(($parts -join "`n"))
    ([BitConverter]::ToString([Security.Cryptography.SHA256]::Create().ComputeHash($bytes))) -replace '-'
}

$markerPath = git -C $root rev-parse --git-path claude-review-fingerprint
if (-not [IO.Path]::IsPathRooted($markerPath)) { $markerPath = Join-Path $root $markerPath }

if ($Mark) {
    [IO.File]::WriteAllText($markerPath, (Get-Fingerprint))
    Write-Output 'revue enregistrée : le commit est autorisé tant que les fichiers ne changent pas.'
    exit 0
}

$raw = [Console]::In.ReadToEnd()
$command = try { ($raw | ConvertFrom-Json).tool_input.command } catch { $null }
if (-not $command -or $command -notmatch '(?m)(^|[;&|]\s*)git(\s+-[-\w]+(\s+[^-\s]\S*)?)*\s+commit(\s|$)') { exit 0 }
if (-not (Get-ChangedFiles | Where-Object { $_ -match $codePattern })) { exit 0 }

$reviewed = if (Test-Path $markerPath) { [IO.File]::ReadAllText($markerPath).Trim() } else { '' }
if ($reviewed -eq (Get-Fingerprint)) { exit 0 }

[Console]::Error.WriteLine('Commit refusé : le skill /review-modified-files n''a pas été passé sur l''état actuel des fichiers (avant-commit : formatage, revue boy scout, build, tests). Le lancer, puis recommiter. Toute modification après la revue demande de la relancer.')
exit 2
