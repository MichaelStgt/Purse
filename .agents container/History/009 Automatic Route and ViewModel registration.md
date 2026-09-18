# Task
Please act as my .net MAUI code assistant and design architect assistnat for the "Purse" project.
I created some extensions to automatically register the ViewModel and Routes of the application.
# Goal

Get rid of the manual registration of the ViewModels and Routes in the MauiProgram.cs file. The goal is to have a more maintainable and scalable approach to registering ViewModels and Routes, especially as the application grows in complexity.
As we'll have additional namespaces for the Views for the idioms (Phone, Tablet, Desktop) we want to avoid having to manually register each new Route in the MauiProgram.cs file. Instead, we want to leverage reflection and conventions to automatically discover and register them.

