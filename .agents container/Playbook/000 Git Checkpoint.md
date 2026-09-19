# Task: Secure Stable State and Prepare Next Branch

## Objective
The current application state has been tested and confirmed stable. Commit the current feature, merge it securely into the `master` branch, push to GitHub, and create a clean branch for the next development phase.

## Execution Steps

### 1. Secure Current Changes
*   Open the terminal in the repository root (`C:\Repos\040 MauiNet10\Purse`).
*   Stage all modified, deleted, and untracked files: `git add .`
*   Commit the changes: `git commit -m "chore: nuke heavy reference folders and recover stable baseline"`

### 2. Merge to Master
*   Switch to the main branch: `git checkout master`
*   Merge the feature branch into master: `git merge -` (merges the previous branch)
*   Push the stable master branch to GitHub: `git push origin master`

### 3. Initialize Next Sandbox
*   Create and checkout the new branch for the upcoming task: `git checkout -b feature/034-remove-planned-amount`
*   Output a confirmation message stating the current active branch.

## Expected Outcome
All local changes are committed and safely pushed to the `master` branch on GitHub. The repository is now checked out to a new `feature/` branch, providing a clean, isolated sandbox for the next playbook.