1. Run publish_linux_gameservices.bat
2. Copy `AlcatrazGameServices\bin\release\net5.0\ubuntu.18.04-x64\publish` to your server
3. Configure your `appsettings.json`
4. Create service:

sudo nano /etc/systemd/system/kestrel-alcatraz.service

```
[Unit]
Description=Alcatraz Rendez-Vous server

[Service]
WorkingDirectory=/home/deployer/publish
ExecStart=/home/deployer/publish/AlcatrazGameServices
Restart=always
RestartSec=10
SyslogIdentifier=alcatraz-rdv
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS="http://0.0.0.0:80"

[Install]
WantedBy=multi-user.target
```

5. Start service
sudo systemctl enable kestrel-alcatraz.service
sudo systemctl start kestrel-alcatraz.service
