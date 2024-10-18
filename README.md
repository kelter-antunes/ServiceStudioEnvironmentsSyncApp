# ServiceStudio Environments Sync App

![GitHub Releases](https://img.shields.io/github/downloads/kelter-antunes/ServiceStudioEnvironmentsSyncApp/total.svg)
![License](https://img.shields.io/github/license/kelter-antunes/ServiceStudioEnvironmentsSyncApp.svg)

## Table of Contents

- [ServiceStudio Environments Sync App](#servicestudio-environments-sync-app)
  - [Table of Contents](#table-of-contents)
  - [About the Project](#about-the-project)
  - [Features](#features)
  - [Requirements](#requirements)
  - [Installation](#installation)
    - [Download](#download)
    - [Configuration](#configuration)
  - [Server Integration](#server-integration)
    - [Download from OutSystems Forge](#download-from-outsystems-forge)
    - [Install Server-Side Application](#install-server-side-application)
    - [Obtain API Key](#obtain-api-key)
    - [Configure `Settings.json`](#configure-settingsjson)
    - [Run the Sync App](#run-the-sync-app)
  - [Usage](#usage)
  - [Contributing](#contributing)
    - [Creating a Local Development Environment](#creating-a-local-development-environment)
      - [Prerequisites](#prerequisites)
      - [Clone the Repository](#clone-the-repository)
      - [Build the Project](#build-the-project)
      - [Run the Application](#run-the-application)
      - [Making Changes](#making-changes)
      - [Submitting Contributions](#submitting-contributions)
  - [License](#license)
  - [Contact](#contact)

## About the Project

[ServiceStudio Environments Sync App](https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp) is a lightweight Windows application designed to synchronize ServiceStudio and ODCStudio saved environments in the cloud. Running silently in the background, it ensures that your development environments remain consistent and up-to-date with minimal user intervention. The application operates through a system tray icon, providing easy access to its status and settings.

Additionally, this app integrates with a server-side component available on the OutSystems Forge, enabling centralized management and synchronization of environments across different installations.

## Features

- **Automatic Synchronization:** Syncs ServiceStudio and ODCStudio server settings at user-defined intervals.
- **System Tray Integration:** Runs in the background with a convenient tray icon for easy access.
- **Configurable Settings:** Customize synchronization intervals, sensitive information handling, log retention, and auto-start preferences via `Settings.json`.
- **Logging:** Maintains detailed logs with configurable retention periods to track synchronization activities and issues.
- **Auto-Start Capability:** Option to launch the app automatically upon system startup.
- **Server Integration:** Connects to a server-side application for centralized synchronization management.

## Requirements

- **Operating System:** Windows 10 or later (64-bit recommended)
- **.NET Runtime:** [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- **Permissions:** Read/write access to ServiceStudio and ODCStudio configuration directories. Administrative privileges may be required for certain operations, such as setting up auto-start.
- **OutSystems Personal Environment:** Required for installing the server-side application.

## Installation

### Download

1. **Navigate to Releases:**
   - Visit the [GitHub Releases Page](https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp/releases).

2. **Download the Latest Release:**
   - Download the latest `.zip` file containing the bundled executable and a sample `Settings.json`.


### Configuration

1. **Extract the Files:**
   - Extract the contents of the downloaded `.zip` file to your desired installation directory.

2. **Review `Settings.json`:**
   - `Settings.json` is located alongside the `ServiceStudioEnvironmentsSyncApp.exe` file.
   - Open `Settings.json` with a text editor (e.g., Notepad) to customize the configuration according to your environment.

   ```json
   {
       "ServiceStudioPath": "C:\\Path\\To\\ServiceStudio",
       "OdCStudioPath": "C:\\Path\\To\\ODCStudio",
       "ServerUrl": "https://your-sync-server.com/api/sync",
       "SyncIntervalInMinutes": 5,
       "SyncSensitiveInfo": true,
       "LogRetentionInDays": 30,
       "AutoStart": false,
       "ApiKey": "YOUR_API_KEY_HERE"
   }
   ```

   - **Parameters:**
     - `ServiceStudioPath`: Absolute path to your ServiceStudio settings directory.
     - `OdCStudioPath`: Absolute path to your ODCStudio settings directory.
     - `ServerUrl`: The endpoint URL where synchronization data will be sent and received.
     - `SyncIntervalInMinutes`: Time interval (in minutes) between each synchronization attempt.
     - `SyncSensitiveInfo`: Whether to include sensitive information (e.g., passwords) during synchronization.
     - `LogRetentionInDays`: Number of days to retain log files before automatic deletion.
     - `AutoStart`: Set to `true` to enable the application to start automatically with Windows.
     - `ApiKey`: The API Key obtained from the server-side application (required for synchronization).

3. **Adjust Permissions (If Necessary):**
   - Ensure that the application has the necessary permissions to read and write to the specified ServiceStudio and ODCStudio directories.
   - If you encounter permission issues, consider running the application as an administrator.

## Server Integration

To enable centralized synchronization, you need to set up the server-side application available on the OutSystems Forge. Follow the steps below to integrate the sync app with the server.

### Download from OutSystems Forge

1. **Visit the OutSystems Forge:**
   - Go to the [ServiceStudio Environments Sync Component](https://www.outsystems.com/forge/component-overview/XXXX/servicestudio-environments) on OutSystems Forge. *(Replace `XXXX` with the actual component ID when available)*

2. **Download the Component:**
   - Click the **Download** button to get the server-side application package.

### Install Server-Side Application

1. **Open OutSystems Personal Environment:**
   - Launch your OutSystems Personal Environment where you want to install the server-side application.

2. **Import the Component:**
   - Navigate to the **Forge** tab within your OutSystems environment.
   - Click on **Upload Package** or **Import Component** and select the downloaded component file.

3. **Deploy the Component:**
   - Follow the on-screen instructions to deploy the server-side application.
   - Ensure that the deployment is successful and that the application is running correctly.

### Obtain API Key

1. **Access the Server-Side Application:**
   - Once deployed, open the server-side application within your OutSystems environment.

2. **Generate or Retrieve API Key:**
   - Navigate to the settings or configuration section of the server-side application.
   - Click on **Generate API Key** or follow the provided instructions to obtain your unique API Key.
   - **Important:** Keep your API Key secure. Do not share it publicly.

### Configure `Settings.json`

1. **Edit `Settings.json`:**
   - Open the `Settings.json` file located alongside the `ServiceStudioEnvironmentsSyncApp.exe` executable.

2. **Add the API Key:**
   - Insert your obtained API Key into the `ApiKey` field.

   ```json
   {
       "ServiceStudioPath": "C:\\Path\\To\\ServiceStudio",
       "OdCStudioPath": "C:\\Path\\To\\ODCStudio",
       "ServerUrl": "https://your-sync-server.com/api/sync",
       "SyncIntervalInMinutes": 5,
       "SyncSensitiveInfo": true,
       "LogRetentionInDays": 30,
       "AutoStart": false,
       "ApiKey": "YOUR_ACTUAL_API_KEY"
   }
   ```

   - **Ensure Accuracy:** Make sure that the `ServerUrl` and `ApiKey` are correctly entered to establish a successful connection between the sync app and the server.

3. **Save the Configuration:**
   - After updating, save the `Settings.json` file to apply the changes.

### Run the Sync App

1. **Launch the Application:**
   - Double-click on `ServiceStudioEnvironmentsSyncApp.exe` to start the application.
   - The application will run silently in the background and place an icon in the system tray.

2. **Verify Synchronization:**
   - Ensure that the first synchronization occurs without errors.
   - Check the `Logs` directory for any log files indicating successful synchronization or errors.


## Usage

1. **Launch the Application:**
   - Double-click on `ServiceStudioEnvironmentsSyncApp.exe` to start the application.
   - The application will run silently in the background and place an icon in the system tray.

2. **Accessing the System Tray Icon:**
   - Locate the application’s icon in the system tray (usually at the bottom-right corner of your screen).
   - **Right-Click Menu Options:**
     - **Status:** Opens a window displaying the latest synchronization status, errors, and application details.
     - **Exit:** Closes the application.


3. **Monitoring Synchronization:**
   - The system tray tooltip provides real-time information about the last synchronization time and any encountered errors.
   - For detailed status, select the **Status** option from the tray icon’s context menu.

4. **Updating Settings:**
   - To modify synchronization settings, edit the `Settings.json` file and restart the application to apply changes.

5. **Logs:**
   - Log files are stored in the `Logs` directory within the installation folder.
   - Logs help in troubleshooting and monitoring the application's activities.

## Contributing

Contributions are welcome! Whether you're fixing bugs, improving documentation, or suggesting new features, your input is valued.

### Creating a Local Development Environment

Follow these steps to set up a local development environment for **ServiceStudio Environments Sync App**:

#### Prerequisites

- **Operating System:** Windows 10 or later
- **.NET SDK:** [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- **IDE:** [Visual Studio 2022](https://visualstudio.microsoft.com/) or later with .NET desktop development workload installed

#### Clone the Repository

1. **Open Terminal or Command Prompt:**

2. **Clone the Repository:**

   ```bash
   git clone https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp.git
   ```

3. **Navigate to the Project Directory:**

   ```bash
   cd ServiceStudioEnvironmentsSyncApp
   ```

#### Build the Project

1. **Restore Dependencies:**

   ```bash
   dotnet restore
   ```

2. **Build the Project:**

   ```bash
   dotnet build -c Release
   ```

#### Run the Application

1. **Navigate to the Build Output Directory:**

   ```bash
   cd bin/Release/net6.0-windows/win-x64/
   ```

2. **Ensure `Settings.json` is Configured:**
   - Modify `Settings.json` as per your environment or use the provided sample.

3. **Run the Executable:**

   ```bash
   ServiceStudioEnvironmentsSyncApp.exe
   ```

   - The application will start silently and appear in the system tray.

#### Making Changes

- **Code Modifications:**
  - Open the project in your preferred IDE (e.g., Visual Studio).
  - Make the necessary code changes, add features, or fix issues.

- **Testing:**
  - After making changes, rebuild the project and run the executable to test your modifications.

#### Submitting Contributions

1. **Fork the Repository:**
   - Click the **Fork** button on the [GitHub repository page](https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp).

2. **Create a Feature Branch:**

   ```bash
   git checkout -b feature/YourFeatureName
   ```

3. **Commit Your Changes:**

   ```bash
   git commit -m "Add your detailed commit message here"
   ```

4. **Push to GitHub:**

   ```bash
   git push origin feature/YourFeatureName
   ```

5. **Open a Pull Request:**
   - Navigate to the original repository on GitHub.
   - Click **Compare & pull request** for your feature branch.
   - Provide a descriptive title and detailed description of your changes.
   - Submit the pull request for review.

## License

Distributed under the [MIT License](LICENSE).



## Contact

Miguel 'Kelter' Antunes - [@miguel_kelter](https://x.com/miguel_kelter) 

Windows client app link: [https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp](https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp)

Server app on OutSystems Forge: [https://www.outsystems.com/forge/component-overview/XXXX/servicestudio-environments](https://www.outsystems.com/forge/component-overview/XXXX/servicestudio-environments)
