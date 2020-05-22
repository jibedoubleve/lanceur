# Table of content

- [Table of content](#table-of-content)
- [Lanceur](#lanceur)
- [What does it do?](#what-does-it-do)
- [How does it work?](#how-does-it-work)
  - [What is a `command line`?](#what-is-a-command-line)
  - [Create a new keyword](#create-a-new-keyword)
    - [Going to the configuration window](#going-to-the-configuration-window)
    - [With the keyword `add`](#with-the-keyword-add)
    - [Create shortcuts (UI)](#create-shortcuts-ui)
  - [Predifined keywords](#predifined-keywords)
  - [Replacement macros](#replacement-macros)
- [Print screens](#print-screens)
- [Acknowledgement](#acknowledgement)

# Lanceur
This is a free adaptation of Slickrun written in .NET and with features inspired by Wox

# What does it do?

Make a list of shortcuts, configure them and earn a lot of time by just typing the shortcut and press `ENTER`

# How does it work?

To display the window, by default, the shortcut is `ctrl + alt + space` 

> `ctrl + alt` has the same behaviour as [AltGr](https://en.wikipedia.org/wiki/AltGr_key)

![img](/doc/assets/Lanceur-UI_parts.png)

When you've used the shortcut, a window appears.
* In the searchbox (point 1), enter the keyword you want to execute
* You'll see as you type the results that correspond to what you're typing (point 2)
* You can type `ENTER` to execute the first element of the list
* OR you can click on the element you want to execute

## What is a `command line`?

> `command` [space] `parameters`

For instance, I've configured a google search as follow `? my_search`. In other words, if I want to search 'aeroplane' in google, I write `? aeroplane`.

Here's how the command is configured
| keyword | file name                                 |
| ------- | ----------------------------------------- |
| ?       | https://www.google.com/search?hl=en&q=\$W\$ |

> the use of \$W$\ is defined [HERE](#replacement-macros)

This command line is divided as follow:
| item      | value      |
| --------- | ---------- |
| ?         | Command    |
| aeroplane | Parameters |



## Create a new keyword

There are two ways to create a new shortcut

### Going to the configuration window

1. Type `setup`
2. Go to the tab `keywords`

### With the keyword `add`

Type `Add <the_keyword_you_want>`, a window appear:

### Create shortcuts (UI)

![img](/doc/assets/Lanceur-UI_add_keyword.png)

1. Type the name of the _shortcut_ you want to configure. 
    > You can set as much keywords as you want
2. Set the _path_ of the shortcut. 
    > The path can be:
    >  * the path of an executable
    >  * a path to a directory
    >  * an URL
3. As a convenience you can use the _cross_ to infer the executable.
    > Drag and drop the cross on a window and _Lanceur_ will infer the path of the executable
4. Add the argument that will be used when launching the executable
5. `RunAs` can be:
    * `Admin`: launch the application with administration privileges
    * `CurrentUser`: launch the application with the privilege of the current user
6. `StartMode` can be:
    * `Default`: keep default configuration of the app
    * `Maximized`: start in fullscreen
    * `Minimized`: start minimised in the taskbar
7. The _working directory_ is the path that will be specified to the program as the working directory when starting it.
8. Some notes user can add to the shortcut.

## Predifined keywords

Some keyword are reserved and have some specific behaviour:

| keyword      | explanation                                           | parameters                       |
| ------------ | ----------------------------------------------------- | -------------------------------- |
| `add`        | Create a new keyword.                                 | The name of the keyword you want |
| `centre`     | Centre the window in the middle of the main screen    | _N.A._                           |
| `clear`      | Clears the database. :exclamation: Erase the database | _N.A._                           |
| `guid`       | Put a new guid into the clipboard                     | _N.A._                           |
| `import`     | Automatically import data from __Slickrun__           | _N.A._                           |
| `quit`       | Quit the application                                  | _N.A._                           |
| `sessions`   | List all the sessions                                 | _N.A._                           |
| `switch`     | Switch to another session                             | The name of the session          |
| `setup`      | Opens the setup window                                | _N.A._                           |
| `statistics` | Displays statistics on the usage                      | _N.A._                           |
| `version`    | Displays the version of Lanceur                       | _N.A._                           |

## Replacement macros

Any occurence of these macro in the `File Name` text box will be replace as follow.

| macro | explanation                                                                      |
| ----- | -------------------------------------------------------------------------------- |
| \$C\$ | is replaced with contents of the Clipboard.                                      |
| \$I\$ | will replace the \$I\$ with typed parameters                                     |
| \$W\$ | will replace the \$I\$ with typed parameters in the format  most web URLs expect |


# Print screens

![img](/doc/assets/Lanceur.png)


# Acknowledgement
* [Application icon](https://fr.seaicons.com/le-lanceur-icone-2)
* [SlickRun](https://bayden.com/SlickRun/) inspired me to build this application
* [Wox](https://github.com/Wox-launcher/Wox) inspired me some features
