# Workflow & Debugging Notification Guidelines

## Always Clear Tasks Before Releasing to User
- Whenever background commands, compilation builds (`dotnet build`), or code generation tasks complete, always verify clean build status.
- Once all build and execution steps are finished and verified, **explicitly and clearly inform the user that all tasks are finished and the application is ready to run and debug**.

## Ready to Debug Message Pattern
- At the end of every response where code changes or compilation tasks have concluded successfully, include a dedicated notification section:
  > **App Status:** Ready to debug! All background tasks and builds have completed successfully.

## Handling File Lock and Build Errors (MAUIR0001, XARDF7024)
If a build or compilation fails with a file lock error, "directory not empty" error, or specifically `MAUIR0001` or `XARDF7024`, do not attempt to fix the code. These are environment locks caused by background language servers. 

When you encounter these errors, you must immediately execute the cleanup script located at `\.agents container\Scripts\Clear-Locks.ps1` via the terminal. Once the script completes, retry the build command.