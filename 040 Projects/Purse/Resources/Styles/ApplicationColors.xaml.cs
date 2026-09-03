// <copyright file="ApplicationColors.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.Resources
{
    /// <summary>
    /// The ApplicationColors.
    /// </summary>
    public partial class ApplicationColors : ResourceDictionary
    {
        /// <summary>
        /// Defines the current.
        /// </summary>
        private static ApplicationColors current = new ApplicationColors();

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>An ApplicationColors</value>
        public static ApplicationColors Current
        {
            get
            {
                //if (current == null)
                //{
                //    current = new ApplicationColors();
                //}
                return current;
            }
        }

        /// <summary>
        /// Gets or sets the ColorCollection.
        /// </summary>
        public List<Color> ColorCollection
        {
            get; set;
        }

        /// <summary>
        /// Get the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>A Color</returns>
        public Color? GetColor(Color color)
        {
            Color? c = this.ColorCollection.FirstOrDefault(fod => fod.Equals(color));
            return c;
        }

        /// <summary>
        /// Get the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>A string?</returns>
        public string? GetColor(string color)
        {
            Color? c = this.ColorCollection.FirstOrDefault(fod => fod.ToHex().Equals(color));

            return c == null ? string.Empty : c.ToHex();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationColors"/> class.
        /// </summary>
        public ApplicationColors()
        {
            this.InitializeComponent();
            var colorArray = new Color[]
            {
                  Colors.Orange,
                  Colors.DarkOrange,
                  Colors.OrangeRed,
                  Colors.PaleVioletRed,
                  Colors.IndianRed,
                  Colors.Magenta,
                  Colors.OrangeRed,
                  Colors.MediumVioletRed,
                  Colors.Red,
                  Colors.DarkRed,
                  Colors.DarkMagenta,
                  Colors.BlueViolet,
                  Colors.AliceBlue,
                  Colors.LightBlue,
                  Colors.CadetBlue,
                  Colors.CornflowerBlue,
                  Colors.DeepSkyBlue,
                  Colors.Blue,
                  Colors.DarkBlue,
                  Colors.DarkSlateBlue,
                  Colors.Yellow,
                  Colors.LightGreen,
                  Colors.GreenYellow,
                  Colors.Lime,
                  Colors.LimeGreen,
                  Colors.Green,
                  Colors.DarkGreen,
                  Colors.SandyBrown,
                  Colors.RosyBrown,
                  Colors.SaddleBrown,
            };

            this.ColorCollection = [.. colorArray];
        }
    }
}