<#
.SYNOPSIS
  Applique l'.editorconfig du dépôt aux fichiers texte que dotnet format ne traite pas (.razor, .html, .css, .js, .json, .md, .yml, .csproj...).
.EXAMPLE
  .\format-files.ps1 Web/Pages/Index.razor Web/wwwroot/app.css
.NOTES
  Appelé par le hook pre-commit, depuis la racine du dépôt, avec les chemins relatifs des fichiers indexés.
  Lit l'.editorconfig racine et applique : end_of_line, trim_trailing_whitespace, insert_final_newline,
  et la conversion des tabulations de début de ligne en espaces (indent_style = space ; ignorée pour .md, .csv, .tsv).
  Ne change ni l'encodage ni le BOM, ne réindente pas, ignore les fichiers binaires et non UTF-8.
#>
param([Parameter(ValueFromRemainingArguments)][string[]]$Files)

$ErrorActionPreference = 'Stop'
$editorConfig = '.editorconfig'
if (-not $Files -or -not (Test-Path $editorConfig)) { exit 0 }

function ConvertTo-Regex([string]$glob) {
    $anchored = $glob.Contains('/')
    $glob = $glob.TrimStart('/')
    $regex = New-Object Text.StringBuilder
    $braces = 0
    for ($i = 0; $i -lt $glob.Length; $i++) {
        $c = $glob[$i]
        if ($c -eq '*' -and $i + 1 -lt $glob.Length -and $glob[$i + 1] -eq '*') { [void]$regex.Append('.*'); $i++ }
        elseif ($c -eq '*') { [void]$regex.Append('[^/]*') }
        elseif ($c -eq '?') { [void]$regex.Append('[^/]') }
        elseif ($c -eq '{') { [void]$regex.Append('('); $braces++ }
        elseif ($c -eq '}' -and $braces -gt 0) { [void]$regex.Append(')'); $braces-- }
        elseif ($c -eq ',' -and $braces -gt 0) { [void]$regex.Append('|') }
        elseif ($c -eq '[' -and $glob.IndexOf(']', $i) -gt $i) {
            $end = $glob.IndexOf(']', $i)
            [void]$regex.Append('[' + $glob.Substring($i + 1, $end - $i - 1).Replace('\', '\') -replace '^!', '^').Append(']')
            $i = $end
        }
        else { [void]$regex.Append([regex]::Escape([string]$c)) }
    }
    $prefix = if ($anchored) { '^' } else { '^(.*/)?' }
    "$prefix$regex$"
}

$sections = @()
foreach ($line in Get-Content $editorConfig -Encoding UTF8) {
    $text = $line.Trim()
    if (-not $text -or $text[0] -eq '#' -or $text[0] -eq ';') { continue }
    if ($text -match '^\[(.+)\]$') { $current = @{ Regex = ConvertTo-Regex $Matches[1]; Properties = @{} }; $sections += $current }
    elseif ($text -match '^([^=]+?)\s*=\s*(.*)$' -and $current) { $current.Properties[$Matches[1].ToLowerInvariant()] = $Matches[2].ToLowerInvariant() }
}

function Get-Properties([string]$path) {
    $properties = @{}
    foreach ($section in $sections) { if ($path -match $section.Regex) { foreach ($key in $section.Properties.Keys) { $properties[$key] = $section.Properties[$key] } } }
    $properties
}

$strictUtf8 = New-Object Text.UTF8Encoding($false, $true)
foreach ($file in $Files) {
    if (-not (Test-Path -LiteralPath $file -PathType Leaf)) { continue }
    $bytes = [IO.File]::ReadAllBytes((Resolve-Path -LiteralPath $file).Path)
    if ([Array]::IndexOf($bytes, [byte]0) -ge 0) { continue }
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    try { $original = $strictUtf8.GetString($bytes, [int]$hasBom * 3, $bytes.Length - [int]$hasBom * 3) }
    catch { Write-Warning "ignoré (non UTF-8) : $file"; continue }

    $properties = Get-Properties $file
    $eol = $properties['end_of_line']
    $text = if ($eol) { $original -replace "`r`n?", "`n" } else { $original }
    if ($properties['trim_trailing_whitespace'] -eq 'true') { $text = $text -replace '(?m)[ \t]+(?=\r?$)', '' }
    if ($properties['insert_final_newline'] -eq 'true' -and $text.Length -gt 0 -and $text -notmatch '[\r\n]$') { $text += if ($text.Contains("`r`n")) { "`r`n" } else { "`n" } }
    if ($properties['indent_style'] -eq 'space' -and $file -notmatch '\.(md|csv|tsv)$') {
        $size = if ($properties['indent_size'] -match '^\d+$') { [int]$properties['indent_size'] } else { 4 }
        $text = [regex]::Replace($text, '(?m)^\t+', { param($m) ' ' * ($m.Length * $size) })
    }
    if ($eol -eq 'crlf') { $text = $text.Replace("`n", "`r`n") }
    elseif ($eol -eq 'cr') { $text = $text.Replace("`n", "`r") }

    if ($text -ceq $original) { continue }
    [IO.File]::WriteAllText((Resolve-Path -LiteralPath $file).Path, $text, (New-Object Text.UTF8Encoding($hasBom)))
    Write-Host "formaté : $file"
}
