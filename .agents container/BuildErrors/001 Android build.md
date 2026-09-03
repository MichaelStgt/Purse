# Task:

6>         Purse -> C:\Repos\040 MauiNet10\Purse\040 Projects\Purse\bin\Debug\net10.0-windows10.0.19041.0\win-x64\Purse.dll
6>     1>obj\Debug\net10.0-android\android\src\crc64338477404e88479c\GenericAnimatorListener.java(50,14): javac.exe error JAVAC0000:  warning: [removal] finalize() in Object has been deprecated and marked for removal
6>obj\Debug\net10.0-android\android\src\crc64338477404e88479c\GenericAnimatorListener.java(50,14): javac.exe error JAVAC0000: 	public void finalize ()
6>obj\Debug\net10.0-android\android\src\crc64338477404e88479c\GenericAnimatorListener.java(50,14): javac.exe error JAVAC0000:
6>     1>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnChangeListenerImplementor.java(4,8): javac.exe error JAVAC0000:  error: BaseOnChangeListenerImplementor is not abstract and does not override abstract method onValueChange(Object,float,boolean) in BaseOnChangeListener
6>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnChangeListenerImplementor.java(4,8): javac.exe error JAVAC0000: public class BaseOnChangeListenerImplementor
6>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnChangeListenerImplementor.java(4,8): javac.exe error JAVAC0000:
6>     1>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnSliderTouchListenerImplementor.java(4,8): javac.exe error JAVAC0000:  error: BaseOnSliderTouchListenerImplementor is not abstract and does not override abstract method onStopTrackingTouch(Object) in BaseOnSliderTouchListener
6>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnSliderTouchListenerImplementor.java(4,8): javac.exe error JAVAC0000: public class BaseOnSliderTouchListenerImplementor
6>obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnSliderTouchListenerImplementor.java(4,8): javac.exe error JAVAC0000:
6>     1>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: obj\Debug\net10.0-android\android\src\crc64338477404e88479c\GenericAnimatorListener.java:50: warning: [removal] finalize() in Object has been deprecated and marked for removal
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: 	public void finalize ()
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: 	            ^
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnChangeListenerImplementor.java:4: error: BaseOnChangeListenerImplementor is not abstract and does not override abstract method onValueChange(Object,float,boolean) in BaseOnChangeListener
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: public class BaseOnChangeListenerImplementor
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000:        ^
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: obj\Debug\net10.0-android\android\src\mono\com\google\android\material\slider\BaseOnSliderTouchListenerImplementor.java:4: error: BaseOnSliderTouchListenerImplementor is not abstract and does not override abstract method onStopTrackingTouch(Object) in BaseOnSliderTouchListener
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: public class BaseOnSliderTouchListenerImplementor
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000:        ^
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: Note: Some input files use or override a deprecated API.
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: Note: Recompile with -Xlint:deprecation for details.
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: Note: Some input files use unchecked or unsafe operations.
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: Note: Recompile with -Xlint:unchecked for details.
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: 2 errors
6>C:\Program Files\dotnet\packs\Microsoft.Android.Sdk.Windows\36.1.43\tools\Xamarin.Android.Javac.targets(161,5): error XAJVC0000: 1 warning

---

## Resolution: ACW Type Erasure Clash Resolved

### Cause of the Error
The `javac.exe` error `JAVAC0000` occurred during the Android Callable Wrapper (ACW) generation phase. The Java compiler detected a **type erasure clash** within the auto-generated Java code for Google Material Slider listener stubs (`BaseOnChangeListenerImplementor` and `BaseOnSliderTouchListenerImplementor`). 

Due to generic type erasure on the JVM, the compiler saw duplicate method signatures (raw `Object` types vs specific generic bounds) that did not correctly override each other. This is a known issue (Issue #1482) in the .NET Android/MAUI bindings for older versions of the Google Material Components library.

### Fix Implemented
1. Added an explicit, conditional package reference in `Purse.csproj` targeting only the Android platform:
   ```xml
   <ItemGroup Condition="'$(TargetFramework)' == 'net10.0-android'">
       <PackageReference Include="Xamarin.Google.Android.Material" Version="1.14.0.4" />
   </ItemGroup>
   ```
2. Restored and compiled the solution for the `-f net10.0-android` target framework. The latest `1.14.0.4` version of the bindings includes the fix (PR #1481) that resolves the type erasure method signature clashes.
3. Verified the build completed successfully with **0 Errors**.
