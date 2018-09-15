# VSTS Pull Request Tool

[![Build status][travis-image]][travis-url]


> A simple command line tool that creates a VSTS Pull Request on your current git repository, implemented in .Net Core.

## Installation

```sh
dotnet tool install -g vsts-pr-tool
```

## Features

* For those who perfer CLI tools, it is a natural next step of the `git commit -> git push` flow
* Identifies from your local git repo the vsts account, repository and default branch to merge to
* Copies the new PR link to your clipboard for fast sharing to your teammates for a code review

## Usage

```sh
# Create a pull request
vsts-pr -t "My awesome feature" -d "A longer description of my feature"

# Create a pull request with default title and message
vsts-pr
```

### CLI Options

* `-t`, `--title` to set the PR title
* `-d`, `--description` to set the PR description

## License

MIT

[travis-image]: https://travis-ci.org/asafst/vsts-pr-tool.svg?branch=master
[travis-url]: https://travis-ci.org/asafst/vsts-pr-tool