# myFirstSchoolProject — Local run + Cloudflare Tunnel

Quick steps to expose the API over a public Cloudflare Tunnel (no domain required).

1) Start the API locally (terminal A):

```powershell
cd C:\Users\hp\source\repos\myFirstSchoolProject
dotnet run --project "myFirstSchoolProject.csproj" --urls "http://localhost:5150"
```

2) In a separate terminal (terminal B) run the tunnel (requires `cloudflared`):

```powershell
# start the tunnel (shows a public URL like https://abc123.trycloudflare.com)
.
\scripts\run_tunnel.ps1
```

3) Use the public URL returned by cloudflared to access the API and Swagger:

- Swagger: `https://<your-tunnel>.trycloudflare.com/swagger`
- Login: `POST https://<your-tunnel>.trycloudflare.com/api/Auth/login`

Notes
- `Program.cs` now registers a permissive CORS policy (`AllowAll`) so external callers can reach the API while testing. For production replace `AllowAnyOrigin()` with a specific origin.
- Protect public endpoints using Cloudflare Access, API tokens, or keep JWT authentication for sensitive routes.
