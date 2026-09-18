// <copyright file="AmountToIconConverter.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Converters
{
    using Microsoft.Maui.Controls;
    using Purse.Resources;
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts a net amount value to a Material Symbol icon glyph (upward arrow for >= 0, downward arrow for < 0).
    /// </summary>
    public class AmountToIconConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            decimal amount = 0m;
            if (value is decimal d)
            {
                amount = d;
            }
            else if (value is double db)
            {
                amount = (decimal)db;
            }
            else if (value is float f)
            {
                amount = (decimal)f;
            }
            else if (value is int i)
            {
                amount = i;
            }
            else if (value is long l)
            {
                amount = l;
            }
            else if (value is string s && decimal.TryParse(s, NumberStyles.Any, culture, out var parsed))
            {
                amount = parsed;
            }

            return amount >= 0 ? MaterialSymbols.Arrow_upward : MaterialSymbols.Arrow_downward;
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
