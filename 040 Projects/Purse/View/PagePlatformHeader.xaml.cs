// <copyright file="PagePlatformHeader.xaml.cs" company="Behr, Michael">
// Copyright Behr, Michael.
// All rights reserved.
// Use of this code is subject to the terms of our license.
// See license.txt file in the project root for full license information.
// </copyright>

namespace Purse.View
{
    using LibraryTen.Controls;
    using LibraryTen.Extensions;
    /// <summary>
    /// Defines the <see cref="PagePlatformHeader" />.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePlatformHeader : ContentView
    {
        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();
        //    Page cp = (ContentPage)AppShell.Current.CurrentPage;
        //    if (cp != null)
        //    {
        //        string? s = cp.GetType().FullName;
        //        this.InfoName = s!;
        //    }
        //}
        //protected override void OnParentChanged()
        //{
        //    base.OnParentChanged();

        //}

        //protected override void OnParentSet()
        //{
        //    base.OnParentSet();
        //    if (this.Parent != null)
        //    {
        //        Debug.WriteLine("Parent set");

        //        // var x = this.GetParentPage();
        //        // var x = this.FindOwningPage<Page>();
        //    }
        //}
        #region Fields

        #region IsCollapsedProperty

        /// <summary>
        /// Defines the IsCollapsedProperty.
        /// </summary>
        public static readonly BindableProperty IsCollapsedProperty =
            BindableProperty.Create(
            nameof(IsCollapsed),
            typeof(bool),
            typeof(PagePlatformHeader),
            defaultValue: default,
            propertyChanged: OnIsCollapsedPropertyChanged);

        /// <summary>
        /// Gets or sets the IsCollapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get
            {
                return (bool)this.GetValue(IsCollapsedProperty);
            }

            set
            {
                this.SetValue(IsCollapsedProperty, value);
            }
        }

        /// <summary>
        /// The OnIsCollapsedPropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" /></param>
        /// <param name="oldValue">The oldValue<see cref="object" /></param>
        /// <param name="newValue">The newValue<see cref="object" /></param>
        private static void OnIsCollapsedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnIsCollapsedPropertyChanged((bool)oldValue, (bool)newValue);
                }
            }
        }

        /// <summary>
        /// The OnIsCollapsedPropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="bool" /></param>
        /// <param name="newValue">The newValue<see cref="bool" /></param>
        private void OnIsCollapsedPropertyChanged(bool oldValue, bool newValue)
        {

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.IsCollapsed = !this.IsCollapsed;
        }

        #endregion IsCollapsed


        /// <summary>
        /// Defines the FontSizeProperty.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(PagePlatformHeader),
            defaultValue: default,
            propertyChanged: OnFontSizePropertyChanged);

        /// <summary>
        /// Defines the InfoNameProperty.
        /// </summary>
        public static readonly BindableProperty InfoNameProperty =
            BindableProperty.Create(
            nameof(InfoName),
            typeof(string),
            typeof(PagePlatformHeader),
            defaultValue: default,
            propertyChanged: OnInfoNamePropertyChanged);

        /// <summary>
        /// Defines the PlatformProperty.
        /// </summary>
        public static readonly BindableProperty PlatformProperty =
            BindableProperty.Create(
            nameof(Platform),
            typeof(string),
            typeof(PagePlatformHeader),
            defaultValue: InitializePlatformInfo,
            propertyChanged: OnPlatformPropertyChanged);

        /// <summary>
        /// Defines the TextColorProperty.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(PagePlatformHeader),
            defaultValue: default,
            propertyChanged: OnTextColorPropertyChanged);

        /// <summary>
        /// Defines the ValueProperty.
        /// </summary>
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
            nameof(Value),
            typeof(string),
            typeof(PagePlatformHeader),
            defaultValue: default,
            propertyChanged: OnValuePropertyChanged);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagePlatformHeader"/> class.
        /// </summary>
        public PagePlatformHeader()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the FontSize
        /// Gets or sets a value indicating whether gets or sets FontSize.
        /// </summary>
        public double FontSize
        {
            get
            {
                return (double)this.GetValue(FontSizeProperty);
            }

            set
            {
                this.SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the InfoName
        /// Gets or sets a value indicating whether gets or sets InfoName.
        /// </summary>
        public string InfoName
        {
            get
            {
                return (string)this.GetValue(InfoNameProperty);
            }

            set
            {
                this.SetValue(InfoNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Platform
        /// Gets or sets a value indicating whether gets or sets Platform.
        /// </summary>
        public string Platform
        {
            get
            {
                return (string)this.GetValue(PlatformProperty);
            }

            set
            {
                this.SetValue(PlatformProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the TextColor
        /// Gets or sets a value indicating whether gets or sets TextColor.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)this.GetValue(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Value
        /// Gets or sets a value indicating whether gets or sets Value.
        /// </summary>
        public string Value
        {
            get
            {
                return (string)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Gets the InitializePlatformInfo.
        /// </summary>
        internal static string InitializePlatformInfo
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                // sb.Append($"Model: {DeviceInfo.Current.Model}");
                // sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
                // sb.AppendLine($"SelectedColor: {DeviceInfo.Current.SelectedColor}");
                // sb.AppendLine($"OS Version: {DeviceInfo.Current.VersionString}");
                sb.Append($"Idiom: {DeviceInfo.Current.Idiom}");
                sb.Append($", Platform: {DeviceInfo.Current.Platform}");

                // bool isVirtual = DeviceInfo.Current.DeviceType switch
                // {
                //    DeviceType.Physical => false,
                //    DeviceType.Virtual => true,
                //    _ => false
                // };

                //// sb.AppendLine($", Virtual device? {isVirtual}");
                //sb.Append($", Virtual device? {isVirtual}");

                return sb.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The OnFontSizePropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" />.</param>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnFontSizePropertyChanged((double)oldValue, (double)newValue);
                }
            }
        }

        /// <summary>
        /// The OnInfoNamePropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" />.</param>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private static void OnInfoNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnInfoNamePropertyChanged((string)oldValue, (string)newValue);
                }
            }
        }

        /// <summary>
        /// The OnPlatformPropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" />.</param>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private static void OnPlatformPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnPlatformPropertyChanged((string)oldValue, (string)newValue);
                }
            }
        }

        /// <summary>
        /// The OnTextColorPropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" />.</param>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnTextColorPropertyChanged((Color)oldValue, (Color)newValue);
                }
            }
        }

        /// <summary>
        /// The OnValuePropertyChanged.
        /// </summary>
        /// <param name="bindable">The bindable<see cref="BindableObject" />.</param>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PagePlatformHeader declaringType = (PagePlatformHeader)bindable;

            if (declaringType != null)
            {
                if (newValue != oldValue)
                {
                    declaringType.OnValuePropertyChanged((string)oldValue, (string)newValue);
                }
            }
        }

        /// <summary>
        /// The OnFontSizePropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="double" />.</param>
        /// <param name="newValue">The newValue<see cref="double" />.</param>
        private void OnFontSizePropertyChanged(double oldValue, double newValue)
        {
        }

        /// <summary>
        /// The OnInfoNamePropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="string" />.</param>
        /// <param name="newValue">The newValue<see cref="string" />.</param>
        private void OnInfoNamePropertyChanged(string oldValue, string newValue)
        {
        }

        /// <summary>
        /// The OnPlatformPropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="string" />.</param>
        /// <param name="newValue">The newValue<see cref="string" />.</param>
        private void OnPlatformPropertyChanged(string oldValue, string newValue)
        {
        }

        /// <summary>
        /// The OnTextColorPropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="Color" />.</param>
        /// <param name="newValue">The newValue<see cref="Color" />.</param>
        private void OnTextColorPropertyChanged(Color oldValue, Color newValue)
        {
        }

        /// <summary>
        /// The OnValuePropertyChanged.
        /// </summary>
        /// <param name="oldValue">The oldValue<see cref="object" />.</param>
        /// <param name="newValue">The newValue<see cref="object" />.</param>
        private void OnValuePropertyChanged(string oldValue, string newValue)
        {
        }

        #endregion


    }
}
