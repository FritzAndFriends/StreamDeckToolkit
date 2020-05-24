## Contributing

There are two ways to contribute to this repo, either the code or the docs.

First check for any open [Issues](https://github.com/FritzAndFriends/StreamDeckToolkit/issues) on [GitHub](https://github.com/FritzAndFriends/StreamDeckToolkit) then comment to indicate you're helping out, or raise a new one with details.

### Code

Pull the code and add your necessary changes, then PR back to dev for review.

### Docs

You're in them! To help out fork this repo, install [DocFX](https://dotnet.github.io/docfx/) and update/add any info.

`docfx docfx.json` will build the site, then move to the "_site" folder and run `docfx serve` then open a browser to "http://localhost:8080/" to see your updates.

There are the Articles you can write to explain the toolkit, then the API docs which builds from the src.

The [docfx.json](../docfx.json) file contains a `metadata` which points to the `src` to built to produce

    {
        "metadata": [
        {
            "src": [
                {
                    "files": ["**.csproj"],
                    "src": "../src"
                }
            ],
            "dest": "api"
        }
        ]
    }

### Template

This documentation site is using the [DocFX Material](https://ovasquez.github.io/docfx-material/) [template](https://dotnet.github.io/docfx/templates-and-plugins/templates-dashboard.html).

The [docfx.json](../docfx.json) file contains a `template` that has been updated, alongside the [templates](../tempaltes) folder containing the src of the template.

    "template": [
        "default",
        "templates/material"
    ]

