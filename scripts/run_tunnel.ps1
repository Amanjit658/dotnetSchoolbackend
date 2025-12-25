# Start a Cloudflare Tunnel to expose the local API
# Requires: cloudflared installed and on PATH
# Usage: .\run_tunnel.ps1

if (-not (Get-Command cloudflared -ErrorAction SilentlyContinue)) {
    Write-Host "cloudflared not found on PATH. Install it first: https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/install-and-setup/installation"
    exit 1
}

Write-Host "Starting tunnel to http://localhost:5150 ..."
Write-Host "You will see a public URL (trycloudflare.com) in the cloudflared output. Press Ctrl+C to stop."

cloudflared tunnel --url http://localhost:5150
