# QuickPick
## Overview
QuickPick is a hobby project of mine. When working on multiple 4K monitors, I always feel the windows taskbar is always to far away.
QuickPick brings the taskbar right there where you are, at your mouse-cursor. 


## Features
- Active Application Management: QuickPick keeps track of your active applications, allowing you to switch between them seamlessly.
- More features will come.

## KeyLogger statement:
QuickPick uses a keyboard hook to intercept and manage keyboard input. This is only used to determine the HotKey combination.

## For Developers
### Project Structure
The QuickPick project is organized into several projects.

	- QuickPick		: The launcher project. Sets up the trayicon and the InputHandling for keyboard and mouse.
	- QuickPick.UI		: Contains everything related to the UI part (WPF) of this tool.
	- QuickPick.Utilities	: Contains logic for mouse and keyboard capture, low level application tracking and more.
	- *.Tests		: Unit Tests projects.
 

### Contributing
We welcome contributions from the developer community. If you're interested in contributing, please follow these steps:

Create a new branch for your changes.
Submit a pull request for review.


## Todo:
- Created Installer / Update
- Expand Featurelist
- Provide beta test option
