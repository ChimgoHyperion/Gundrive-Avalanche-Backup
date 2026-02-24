# GunDrive - Multiplayer Shooter Game

A Unity-based multiplayer shooter game featuring Web3 integration, NFT skins, and blockchain-based virtual currency. Built with Unity 2022.3.46f1 and powered by Photon Fusion for networking.

## 🎮 Game Overview

GunDrive is an action-packed multiplayer shooter game that combines traditional gaming mechanics with modern Web3 technology. Players can battle in various maps, customize their characters with NFT skins, and earn virtual currency through gameplay and blockchain transactions.

## ✨ Key Features

### 🎯 Core Gameplay
- **Multiplayer Combat**: Real-time multiplayer battles using Photon Fusion
- **Multiple Maps**: 7 different battle arenas with unique environments
- **Weapon Variety**: Multiple weapon types including rifles, lasers, fireball launchers, and other special weapons
- **Enemy AI**: Intelligent enemy spawning and combat systems
- **Power-ups**: Various power-ups including shields, health packs, and boost packs

### 🌐 Web3 Integration
- **Wallet Connection**: Connect with Web3 wallets using Reown AppKit
- **NFT Skins**: Own and use NFT character skins in-game
- **Virtual Currency**: Earn and spend blockchain-based tokens
- **Marketplace**: Purchase in-game items with cryptocurrency
- **Hedera Testnet**: Built on Hedera blockchain for fast, low-cost transactions

### 🎨 Customization
- **Character Skins**: Multiple character options and NFT-based skins
- **Weapon Customization**: Various weapons with different stats and abilities
- **UI Themes**: Customizable interface elements

### 🏆 Progression System
- **Leaderboards**: Global and local leaderboards via PlayFab
- **XP System**: Highscore points and rewards
- **Achievements**: Various in-game achievements and milestones
- **Tutorial**: Comprehensive tutorial system for new players

## 🛠️ Technical Stack

### Core Technologies
- **Unity Engine**: 2022.3.46f1
- **Photon Fusion**: Multiplayer networking
- **PlayFab**: Backend services and leaderboards
- **Universal Render Pipeline (URP)**: Modern rendering pipeline

### Web3 Technologies
- **Reown AppKit**: Wallet connection and Web3 integration
- **Nethereum**: Ethereum/Hedera blockchain interaction
- **ERC-20**: Token standard for virtual currency
- **ERC-721**: NFT standard for character skins
- **IPFS**: Decentralized storage for NFT metadata
- **Cloudinary**: Image and metadata hosting

### Additional Packages
- **Cinemachine**: Camera system
- **TextMeshPro**: Advanced text rendering
- **Unity Analytics**: Game analytics

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3.46f1 or later
- Visual Studio or compatible IDE
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/gundrive-game.git
   cd gundrive-game
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Add" and select the project folder
   - Open the project with Unity 2022.3.46f1

3. **Configure Web3 Settings**
   - Navigate to `Assets/Scripts/web3/Web3Manager.cs`
   - Update the following configuration:
     - `projectId`: Your Reown project ID
     - `erc721ContractAddressNFT`: Your NFT contract address
     - `tokenContractAddress`: Your ERC-20 token contract address
     - `pinataApiKey` & `pinataSecretApiKey`: Pinata IPFS credentials
     - `cloudName` & `uploadPreset`: Cloudinary configuration

4. **Configure PlayFab**
   - Set up a PlayFab account
   - Update PlayFab settings in `Assets/Scripts/PlayfabManager.cs`

5. **Build and Run**
   - Configure build settings for your target platform
   - Build and deploy the game

## 🎮 Game Scenes

- **Main Menu**: Main game interface and navigation
- **Tutorial**: Interactive tutorial for new players
- **Map Menu**: Map selection interface
- **Map 1-7**: Individual battle arenas
- **Guns Shop**: Weapon purchasing interface
- **Coin and Skin Shop**: Virtual currency and NFT skin marketplace
- **Fusion Multiplayer**: Multiplayer lobby and game rooms

## 🔧 Configuration

### Web3 Configuration
The game requires several Web3 services to be configured:

1. **Reown AppKit**: For wallet connections
2. **Hedera Testnet**: For blockchain transactions
3. **Pinata**: For IPFS storage
4. **Cloudinary**: For image hosting

### PlayFab Configuration
- Set up PlayFab project
- Configure leaderboards
- Set up analytics tracking

## 🎯 Game Mechanics

### Combat System
- **Health System**: Player and enemy health management
- **Weapon System**: Multiple weapon types with different characteristics

### Multiplayer Features
- **Room System**: Create and join multiplayer rooms
- **Synchronization**: Real-time game state synchronization
- **Player Management**: Player spawning and management

### Web3 Features
- **Wallet Integration**: Connect and manage Web3 wallets
- **NFT Management**: View and use owned NFT skins
- **Token Economy**: Earn and spend virtual currency
- **Marketplace**: Purchase items with cryptocurrency

## 📱 Platform Support

- **PC**: Windows, Mac, Linux
- **Mobile**: Android, iOS (with appropriate build settings)
- **WebGL**: Browser-based gameplay

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:
- Create an issue in the GitHub repository
- Contact the development team

## 🔮 Future Features

- Additional game modes
- More weapon types
- Enhanced NFT features
- Cross-platform progression
- Tournament system

## 📊 Project Structure

```
Assets/
├── Scripts/           # Game logic and systems
│   ├── web3/         # Web3 integration scripts
│   ├── multiplayer/  # Multiplayer-related scripts
│   └── gameplay/     # Core gameplay mechanics
├── _Scenes/          # Game scenes
├── Prefabs/          # Reusable game objects
├── Sprites/          # Game art assets
├── Audio Clips/      # Sound effects and music
└── Resources/        # Runtime-loaded assets
```

---

**GunDrive** - Where traditional gaming meets Web3 innovation! 🎮⚡
