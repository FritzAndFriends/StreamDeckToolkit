## Using the Template

Once the template is installed, open a terminal in the folder of your choice and create a new project.

    dotnet new streamdeck-plugin --plugin-name FirstPlugin --uuid com.yourcompany.pluginname.actionname --skipRestore false

Or create a directory in a location of your choice, change to that directory and run the command, which will inherit the directory name as the project name with default values.

    dotnet new streamdeck-plugin
