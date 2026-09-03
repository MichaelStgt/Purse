// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Purse.Data.Services;

public interface IAlertService
{
    Task ShowErrorAlertAsync(string title, string message, string cancel = "OK");
}

public class AlertService : IAlertService
{
    public async Task ShowErrorAlertAsync(string title, string message, string cancel = "OK")
    {
        await Application.Current!.Windows[0].Page!.DisplayAlertAsync(title, message, cancel);
    }
}
