# Calculator

![img](/doc/assets/Plugin_Calculator.png)

Whenever you start the command with `=` it'll activate the calculator.

## Supported parameters

| operator      |      |
| ------------- | ---- |
| addition      | +    |
| substraction  | -    |
| mutiplication | *    |
| division      | /    |
| square root   | sqrt |
| cosinus       | cos  |
| sinus         | sin  |
| power         | pow  |


# Spotify

Press `ss` (*Spotify song*) and see what song is played into spotify

![img](/doc/assets/Plugin_Spotify.png)

## Configure

Just press `ss` and follow the instructions on the screen. Of course, you must have a Spotify account!

# Clipboard

Press `cb` and it will the the clipboard (So far it only save text saved into the clipboard.) If an image is in the clipboard, the command will be ignored

Press `cb l` (Notice the `space`) and you'll list all the clipboard saved


![img](/doc/assets/Plugin_Clipboard.png)

# Evernote

Type `EN` to create a new note into the default notebook

| command                                  | what is does          |
| ---------------------------------------- | --------------------- |
| en text_of_the_tile                      | It creates a note     |
| en -r dd-mm-yyyy text_of_the_title       | It creates a reminder |
| en reminder dd-mm-yyyy text_of_the_title | It creates a reminder |

## Configure

Just type 
`en -c key session_consumer_key host session_consumer_secret`

>So far you have to have an API key you can ask to [Evernote](https://dev.evernote.com/) I'm working on automating the process and use my API key.