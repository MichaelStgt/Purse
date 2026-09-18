// <copyright file="GlobalXmlns.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

// The sample from David Ortinau's blog post on simpler XAML in .NET MAUI 10:
//https://devblogs.microsoft.com/dotnet/simpler-xaml-in-dotnet-maui-10/?hide_banner=true

using XmlnsDefinitionAttribute = Microsoft.Maui.Controls.XmlnsDefinitionAttribute;
using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/maui/global", "global")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/maui/global", "x")]
[assembly: XmlnsPrefix("http://schemas.purse/views/shared", "sharedview")]
[assembly: XmlnsPrefix("http://schemas.purse/views/desktop", "desktop")]
[assembly: XmlnsPrefix("http://schemas.purse/views/tablet", "tablet")]
[assembly: XmlnsPrefix("http://schemas.purse/views/phone", "phone")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Resources")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Converters")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Resources.Strings")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Internal")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Model")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Resources.Generated")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.Shared.Model",
    AssemblyName = "Purse.Shared")]

[assembly: XmlnsDefinition(
    "http://schemas.purse/views/shared",
    "Purse.View")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Purse.ViewModel")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "LibraryTen.Controls",
    AssemblyName = "LibraryTen")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "System.ComponentModel.DataAnnotations",
    AssemblyName = "System.ComponentModel.DataAnnotations")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "LibraryTen.Converter",
    AssemblyName = "LibraryTen")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "LibraryTen.MarkupExtensions", AssemblyName = "LibraryTen")]

[assembly: XmlnsPrefix(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "global")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "Syncfusion.Maui.Inputs", AssemblyName = "Syncfusion.Maui.Inputs")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "CommunityToolkit.Maui.Views", AssemblyName = "CommunityToolkit.Maui")]

[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "http://schemas.microsoft.com/dotnet/2022/maui/toolkit")]