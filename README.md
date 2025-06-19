# P2P Voice Chat App (C#)

A lightweight peer-to-peer voice chat application built with C#.  
It uses **NAudio** for audio capture/playback and **Concentus** (Opus codec) for audio compression.  
Communication is handled over **UDP** using manually exchanged IP and port information with **STUN**-based public IP discovery.

## Features

- Real-time voice chat over the internet
- Peer-to-peer UDP communication
- Opus audio encoding/decoding
- STUN support for public IP discovery
- Manual signaling and port forwarding (no central server)

## Technologies Used

- C# (.NET 8.0)
- Windows Forms
- [NAudio](https://github.com/naudio/NAudio)
- [Concentus](https://github.com/lostromb/concentus)
- [STUN.NET](https://github.com/Ayvan/STUN)

## Setup

1. Clone the repository
2. Open in Visual Studio
3. Restore NuGet packages
4. Build and run the project

## Usage

1. **Port forward UDP port** (e.g., `5500`) on your router
2. Use the STUN feature to get your public IP
3. Share your IP and port with your peer
4. One user presses **Start Receiving**
5. The other user enters target IP/port and presses **Start Sending**

## Notes

- Both users must be on separate networks with port forwarding configured
- UDP traffic on the chosen port must be allowed through Windows Firewall

## License

MIT
