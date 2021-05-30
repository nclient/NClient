# Contributor's Guide

Welcome and thank you for your interest in contributing to the NClient! 👍 There are many ways you can contribute to the project:
* The simplest contribution is to give this project a star ⭐
* Offer PR's to fix bugs or implement new features
* Share your feedback and ideas
* Improve our documentation

## Code Contribution

The contribution process consists of the following steps:
* Fork the repository
* Choose a issue or create your own
* Create a branch to work in
* Make your feature addition or bug fix
* Don't forget the unit tests
* Make sure all tests are passing
* Send a pull request

Before you contribute, read the existing code to see how it is formatted and ensure contributions match the existing style. 
All new features and fixes must include accompanying tests. For more information, see below.

### Git workflow

Our Git workflow is inspired greatly by the 
[GitLab Flow](https://docs.gitlab.com/ee/topics/gitlab_flow.html#environment-branches-with-gitlab-flow).  
The names of branches with features start with `features/`, for fixes use `fixes/`. The naming style is kebab case (kebab-case). 
The main branch name should start with the issue number: `features/10-branch-name`.
Rules for a great git commit message style: capitalize the subject line and use the imperative mood.

### Code Style

Our code tries to follow the Microsoft's C# [guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines). Please note, we use four spaces instead of tabs.    
Before committing, format the text using the `dotnetformat --folder ' command (see [dotnet-format](https://github.com/dotnet/format)).

### Testing

All pull requests must be accompanied by units tests. If it is a new feature, the tests should highlight expected use cases as well as edge cases, if applicable. 
If it is a bugfix, there should be tests that expose the bug in question.

### XML Documentation

Public classes and methods should be documented in English.
