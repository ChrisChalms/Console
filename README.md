# Console

Console is an advanced in-game console for Unity with support viewing Unity's logs, as well as stacktraces. It can also be used for registering and executing commands at runtime e.g. SpawnEnemies 10. [Video of version 0.2](https://youtu.be/1PUk6u48Rak).

## Usage
It's very easy to use and setup. Just copy the root folder 'Console' into your project and navigate to DefaultConsole->Prefabs and drag the Console prefab anywhere in the scene. If there's not an EventSystem in the scene, create one by right clikcing in the hierarchy, selected UI->Event System . That's it! The console will at runtime. You can force desktop or mobile views in the dropdown menu on the prefab, it defaults to autodetect.

On desktop you press the backquote (`) to open/close the console, and on mobile there is a floating button on the left-hand side of the screen that opens the console. You can also move the button on mobile to keep it out of the way.

You can use the up and down arrows on the keyboard, or the quick command buttons on mobile as of v0.2, to quickly enter the previous and next command entered. There's also the TAB button, as well as the quick command button on mobile from v0.2, to fill in a partial entered command. If there's more than one command that can be filled a list of all available commands that match the entered text will be shown

## Commands
There a few default commands that the console comes with, but you can easily add your own. Below are the default commands:

- Console.Log - Logs a message to the console
- Console.LogWarning - Logs a warning to the console
- Console.LogError - Logs an error to the console
- Clear - Clear the view of all logs
- Console.Close - Closes the console window
- Help - Shows the help text of all available commands
- Debug.Log - Log a message to the Unity console
- Debug.LogWarning - Log a warning message to the Unity console
- Debug.LogError - Log an error message to the Unity console
- Debug.LogException - Log an exception to the Unit console
- Debug.Break - Pause the editor
- Quit - Stops the editor playing or quits a standalone build

You can execute the commands by entering them into the console, or via script e.g. 

```c#
    ConsoleCommands.ExecuteCommand("SpawnEnemies",  new string[]{ "10" });
```

You can also add your own commands to suit your needs e.g.

```c#
    ConsoleCommands.AddCommand("Name", methodToCall, "HelpText");

    // Example
    ConsoleCommands.AddCommand("SpawnEnemy", spawnEnemy, "Spawns a basic enemy for testing");

    // Spawn a basic enemy
    private void spawnEnemy(string[] args)
    {
        // Maybe parse some args here to get a more details enemy e.g. level, appearance, number of enemies,  or player offset
        // ...
        
        // Spawn
        var tempEnemy = Instantiate(enemyPrefab, _player.position + _defaultOffset);
        tempEnemy.KillPlayer();
    }
```

## TODO
- ~~Quick command bar on mobile~~
- Make a better example scene
- Scriptable object to change the console's colours
- Scriptable object to chagne the console's icons