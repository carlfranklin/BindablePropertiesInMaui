# Bindable Properties in MAUI

Bindable properties are an essential part of developing a robust user interface in MAUI. They allow you to create properties that can be bound to UI elements, enabling you to respond to user input and update the UI in real-time. With bindable properties, you can also perform validation on user input and display errors in the UI.

> :point_up: **Note**: This demo applies only to MAUI XAML projects. MAUI Blazor doesn't require property change notifications. Instead, it uses it's own binding system which relies on calling `StateHasChanged()` to re-render a component.

## Introduction

In this tutorial, we'll cover the creation and effective usage of bindable properties in your MAUI app. We'll start by tracing the evolution from `INotifyPropertyChanged` to `ObservableObject`, ultimately leading to `BindableProperty`.

By the end of this tutorial, you will have a better understanding of how to create and use bindable properties in MAUI to build robust and responsive user interfaces.

Here is a preview of what the final application will look like:

![Bindable Properties in MAUI](images/52d7531990b2c60a92951a7b40daf391bdbfa3276b6581cdeb554c7c4f11ee7d.png)

Let's dive in!

## Prerequisites

Before starting this tutorial, you should have some experience with C# and MAUI development. You should be familiar with creating and using controls and layouts in MAUI. If you're new to MAUI development, the [Official MAUI documentation](https://docs.microsoft.com/dotnet/maui/) is a great place to start. Additionally, if you have experience with Xamarin.Forms, it will be helpful in understanding some of the concepts in this tutorial.

To follow along with the demo in this tutorial, you will need:

- .NET 7 SDK or later
- Visual Studio 2022
- Basic knowledge of C# and XAML

If you don't have experience with Xamarin.Forms, don't worry, as we will be covering everything you need to know about Bindable Properties in MAUI. If you have these prerequisites covered, let's move on to building our demo application.

### .NET 7.0

Download the latest version of the .NET 7.0 SDK [here](https://dotnet.microsoft.com/en-us/download).

### Visual Studio 2022

To follow along with this demo, I will be using the latest version of Visual Studio 2022. You can download it [here](https://visualstudio.microsoft.com).

### Required Workloads

To build .NET MAUI applications, you will also need the .NET Multi-platform App UI development workload. If you haven't already installed it, you can do so by following these steps:

1. Open Visual Studio Installer and select "Modify" on your installation.
2. In the "Workloads" tab, select the ".NET Multi-platform App UI development" workload.
3. Click "Modify" to start the installation process.

![.NET Multi-platform App UI development](images/34640f10f2d813f245973ddb81ffa401c7366e96e625b3e59c7c51a78bbb2056.png)  

Once the installation is complete, you're ready to start building your .NET MAUI application.

## Demo

In this tutorial, we'll be creating a MAUI application to showcase how to use bindable properties. We will start by using the old way of notifying the UI when a property changes, which is to implement `INotifyPropertyChanged`.

We will show how to use `ObservableObject` to reduce the boilerplate code of using `INotifyPropertyChanged`.

Then, we will move on to the new way of doing this, which is creating a bindable property using `BindableProperty.Create` and binding it to our UI.

Finally, we will look at how implementing a `BindableProperty` in the component, rather than implementing in a model, is the ideal solution for creating component properties.

### Create a .NET MAUI Application

Open Visual Studio 2022 and select "Create a new project".

![Create a new project](images/59cfeada765b0d106cbc12cbc55b517b677ff7316160d3eb75a0541b65b6731a.png)  

Search for ".NET MAUI App" in the search bar and select it.

![.NET MAUI App](images/93aafda7cca158ae7db7b457c494947ca1ae2446329250a1bb86b39eaed45154.png)  

Set the name to **BindablePropertiesInMaui** and click **Next**.

![Name and location](images/1656e70318eafbd8dee014166f1f0a65772d7183a3711f37ddf153f2fa87d662.png)  

In the "Additional information" window, choose the app's target Framework and click "Create" to create the project.

![Additional information](images/2c0044351683c136074708773b5ed14f70a81ab26d0c6a65d0f177d006f7e6ec.png)  

Now, let's run the app to ensure everything is working as expected.

In this case, I will be demonstrating the app on a Windows Machine, but you can run it on your preferred emulator or simulator and expect similar results.

![Windows Machine](images/ea398465f0cf6009872a627d5bb863834df3b9a763ef849b7d303df64a163779.png)  

### Implementing INotifyPropertyChanged

Before we dive into bindable properties, let's take a step back and look at implementing `INotifyPropertyChanged`. This interface is used to notify the UI when a property changes so that it can be updated accordingly.

Let's define a `Person` class with three properties, `FirstName`, `LastName` and `DateOfBirth`. We will also implement the `INotifyPropertyChanged` interface and add a method called `OnPropertyChanged` that raises the `PropertyChanged` event when a property is changed. We use this method in the setters of our properties to notify the UI when a property has changed.

*Person.cs*:

```csharp
using System.ComponentModel;

namespace BindablePropertiesInMaui;

public class Person : INotifyPropertyChanged
{
    public Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        _firstName = firstName;
        _lastName = lastName;
        _dateOfBirth = dateOfBirth;
    }

    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName == value) return;
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            if (_lastName == value) return;
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (_dateOfBirth == value) return;
            _dateOfBirth = value;
            OnPropertyChanged(nameof(DateOfBirth));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, 
          new PropertyChangedEventArgs(propertyName));
    }
}
```

#### Create a PersonComponent

Now let's create a new `PersonComponent` XAML file using a `ContentView` object to represent the UI for the person's information. This component will display the person's information in their respective controls (e.g. Entry for FirstName, Entry for LastName, and DatePicker for DateOfBirth) and allow us to edit.

This component will be used to display the person's information on the main page of the app.

Right-click on the `BindablePropertiesInMaui` project and select Add -> New Item.

Select the ".NET MAUI" option on the left-hand side and then select ".NET MAUI ContentView (XAML)" on the right-hand side. 

Name the file *PersonComponent.xaml* and click the "Add" button.

![.NET MAUI ContentView (XAML)](images/54e70f37561cfe621f8e4de1552f88377f403e7cbd6e93260ab902134316a0a4.png)  

Replace the default XAML code with the following code:

*PersonComponent.xaml*:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
			 x:Class="BindablePropertiesInMaui.PersonComponent">
	<StackLayout MaximumWidthRequest="400">
		<Label Text="Person"
			   FontSize="Medium"
			   Margin="0,24,0,0" />
		<Entry Text="{Binding FirstName}"
			   Placeholder="First Name"
			   Margin="0,24,0,0" />
		<Entry Text="{Binding LastName}"
			   Placeholder="Last Name"
			   Margin="0,24,0,0" />
		<DatePicker Date="{Binding DateOfBirth}"
					Margin="0,24,0,0"
					DateSelected="DatePicker_DateSelected" />
	</StackLayout>
</ContentView>
```

Open the *PersonComponent.xaml.cs* file and replace the default code with the following C# code:

*PersonComponent.xaml.cs*:

```csharp
namespace BindablePropertiesInMaui;

public partial class PersonComponent : ContentView
{
	private Person _person 
        = new("Carl", "Franklin", DateTime.Now );

	public PersonComponent()
	{
		InitializeComponent();
		BindingContext = _person;
	}

	private void DatePicker_DateSelected(object sender, 
        DateChangedEventArgs e)
	{
		_person.DateOfBirth = e.NewDate;
	}
}
```

Note that we're setting the BindingContext to a local `Person` object. 

We are also handling the `DatePicker.DateSelected` event, where we update the `Person`'s `DateOfBirth` property.

Open the *MainPage.xaml* file and replace the default code with the following XAML code:

*MainPage.xaml*:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
			 x:Class="BindablePropertiesInMaui.MainPage"
			 xmlns:local="clr-namespace:BindablePropertiesInMaui">
	<local:PersonComponent/>
</ContentPage>
```

We have added the `local` XAML namespace, and defined a `PersonComponent` on the page.

Open the *MainPage.xaml.cs* file and replace the default code with the following C# code:

```csharp
namespace BindablePropertiesInMaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
}
```

After running the application, you will see that the controls are automatically populated with the data from the `Person` class' constructor `private Person _person = new("Carl", "Franklin", DateTime.Now );` in the *PersonComponent.xaml.cs* class.

![Data Binding Class to UI](images/52d7531990b2c60a92951a7b40daf391bdbfa3276b6581cdeb554c7c4f11ee7d.png)  

Additionally, observe that modifying any of the fields in the user interface will update the corresponding properties

![Data Binding UI to Class](images/963a2e5f3b942ec38ec10b36d656fcd8cbca71587ebe3e76d1edef7f130a50ae.png)  

![DateOfBirth Updated](images/de5ae5fb4244c653ea7cd4b03d4661008a21adf7e7dd9584f1863ec05c446b95.png)  

So far we have:

- Created a `Person` class that implements `INotifyPropertyChanged` with `FirstName`, `LastName`, and `BirthDate` properties.
- Created a `PersonComponent` `ContentView` control to represent a person and bind its properties to the person object.
- Replaced the default `MainPage.xaml` and `MainPage.xaml.cs` code to display the `PersonComponent` object in the UI.

### MVVM Toolkit

The MVVM Toolkit is a set of libraries for building modern, scalable, and easy-to-maintain applications using the Model-View-ViewModel (MVVM) pattern in WPF, WinUI, Xamarin, and MAUI. The toolkit provides a number of features that simplify the development process and make it easier to create maintainable and testable code. Some of the benefits of the MVVM Toolkit include:

- Simplified data binding: The toolkit provides a simplified syntax for data binding, which makes it easy to bind data between the view and the view model without having to write a lot of boilerplate code.

- Commanding: The toolkit provides a Command class that can be used to implement commands in the view model, which can be bound to controls in the view.

- ObservableObject: The toolkit provides an `ObservableObject` class that simplifies the implementation of the `INotifyPropertyChanged` interface, which is required for data binding. The `ObservableObject` class provides a `SetProperty` method that automatically raises the `PropertyChanged` event and updates the value of a property.

In the following demo, we will demonstrate the use of `ObservableObject` provided by the MVVM Toolkit. It is worth noting that we will focus solely on this topic as each benefit of the toolkit deserves a more in-depth discussion.

To learn more about the MVVM Toolkit go to [Introduction to the MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/).

### Implement ObservableObject

Implementing `INotifyPropertyChanged` was the standard way of enabling data binding in .NET for years. However, it required developers to write a lot of repetitive code in each view model or model class to fire property changed events for every property. This led to code that was difficult to maintain and prone to errors.

Eventually, a solution was developed in the form of the `ObservableObject` class. This class simplifies the process of implementing `INotifyPropertyChanged` by handling the event firing for all properties automatically. Developers can simply inherit from this class and define their properties as usual without having to worry about the underlying event handling code.

To use the `ObservableObject` class, you need to install the `CommunityToolkit.Mvvm` NuGet package. You can do this by running the following command in the NuGet Package Console:

```powershell
install-package CommunityToolkit.Mvvm
```

After installing the package, you can use the `ObservableObject` class in your code to reduce the boilerplate code required for implementing the `INotifyPropertyChanged` interface.

Replace the *Person.cs* code with the following code:

*Person.cs*:

```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace BindablePropertiesInMaui;

public class Person : ObservableObject
{
    public Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        _firstName = firstName;
        _lastName = lastName;
        _dateOfBirth = dateOfBirth;
    }

    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => SetProperty(ref _dateOfBirth, value);
    }
}
```

In this example, our `Person` class inherits `ObservableObject`, which implements `INotifyPropertyChanged`.

the `SetProperty` method is used to set the property value and fire the `PropertyChanged` event automatically. By using the `ObservableObject` class, developers can reduce the amount of boilerplate code in their projects and make their code more maintainable and error-free.

After running the application, it can be observed that the data binding functionality still works bidirectionally. Furthermore, it can be noted that the amount of repetitive code has been significantly reduced, providing a more concise and maintainable codebase.

![Data Binding Class to UI](images/b1fd1fe799d6d02efb25abfbc8e8625d39b70a91bd3a80dfd23f45cb4b0239ca.png)  

Observe that modifying any of the fields in the user interface still will update the corresponding properties

![Data Binding UI to Class](images/4f3cae10bb12d69529e601903fd4c02f9526a03391e2e4847e48ce8bc1faddd8.png)  

To learn more about `ObservableObject`, visit the official Microsoft documentation at [ObservableObject](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/observableobject).

### BindableObject and BindableProperty

Having explored the previous approaches that led to the current solution using bindable properties, let's now shift our focus to creating bindable properties since we now understand how to implement `INotifyPropertyChanged` and how to use the `ObservableObject` as a base class.

Open *Person.cs* file.

Remove the `INotifyPropertyChanged` interface and the `OnPropertyChanged` method since we won't need them with the new `BindableProperty`.

Replace each property with a `BindableProperty`. Add the following code for the `FirstName` property:

```csharp
public static readonly BindableProperty FirstNameProperty =
     BindableProperty.Create(nameof(FirstName), typeof(string), typeof(Person), string.Empty, BindingMode.TwoWay);

public string FirstName
{
     get => (string)GetValue(FirstNameProperty);
     set => SetValue(FirstNameProperty, value);
}
```

Repeat the step above for the `LastName` and `DateOfBirth` properties, replacing the existing code for each property with the following:

```csharp
public static readonly BindableProperty LastNameProperty =
     BindableProperty.Create(nameof(LastName), typeof(string), typeof(Person), string.Empty, BindingMode.TwoWay);

public string LastName
{
     get => (string)GetValue(LastNameProperty);
     set => SetValue(LastNameProperty, value);
}

public static readonly BindableProperty DateOfBirthProperty =
     BindableProperty.Create(nameof(DateOfBirth), typeof(DateTime), typeof(Person), DateTime.Now, BindingMode.TwoWay);

public DateTime DateOfBirth
{
     get => (DateTime)GetValue(DateOfBirthProperty);
     set => SetValue(DateOfBirthProperty, value);
}
```

Below is the updated *Person.cs* file with bindable properties for `FirstName`, `LastName`, and `DateOfBirth`:

*Person.cs*:

```csharp
namespace BindablePropertiesInMaui;

public class Person : BindableObject
{
    public static readonly BindableProperty FirstNameProperty = BindableProperty.Create(nameof(FirstName), typeof(string), typeof(Person), string.Empty);
    public static readonly BindableProperty LastNameProperty = BindableProperty.Create(nameof(LastName), typeof(string), typeof(Person), string.Empty);
    public static readonly BindableProperty DateOfBirthProperty = BindableProperty.Create(nameof(DateOfBirth), typeof(DateTime), typeof(Person), DateTime.Now);

    public Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName
    {
        get => (string)GetValue(FirstNameProperty);
        set => SetValue(FirstNameProperty, value);
    }

    public string LastName
    {
        get => (string)GetValue(LastNameProperty);
        set => SetValue(LastNameProperty, value);
    }

    public DateTime DateOfBirth
    {
        get => (DateTime)GetValue(DateOfBirthProperty);
        set => SetValue(DateOfBirthProperty, value);
    }
}
```

Open the *PersonComponent.xaml* file.

Update the `Entry` controls to bind to the new `FirstName`, `LastName` and `DateOfBirth` bindable properties:

Here's the updated *PersonComponent.xaml* file:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="BindablePropertiesInMaui.PersonComponent">
    <StackLayout MaximumWidthRequest="400">
        <Label Text="Person"
               FontSize="Medium"
               Margin="0,24,0,0" />
        <Entry Text="{Binding FirstName, Mode=TwoWay}"
               Placeholder="First Name"
               Margin="0,24,0,0" />
        <Entry Text="{Binding LastName, Mode=TwoWay}"
               Placeholder="Last Name"
               Margin="0,24,0,0" />
        <DatePicker Date="{Binding DateOfBirth, Mode=TwoWay}"
                    Margin="0,24,0,0" />
    </StackLayout>
</ContentView>
```

After running the application, it can be observed that the values hard-coded in `private Person _person = new("Carl", "Franklin", DateTime.Now);` of the `Person` class in the *PersonComponent.xaml.cs* are still binding to the UI.

![Data Binding Class to UI](images/af6cb9849fa7055fbb4422868533ee4f0d4b7a348640b9920c9526278ccdee5a.png)  

Also notice that any UI changes also bind to the code behind.

![Data Binding UI to Class](images/963a2e5f3b942ec38ec10b36d656fcd8cbca71587ebe3e76d1edef7f130a50ae.png)  

![DateOfBirth Updated](images/0e3ae8a4fef4344a36344de34a25f30191c079890090ab6887fe95c5205a72aa.png)  

#### Why should you use BindableObject and BindableProperty?

Wondering which approach to use for data binding in your MAUI application, `INotifyPropertyChanged`, `ObservableObject`, or `BindableProperty`? Let's explore the reasons to choose one over the other.

`INotifyPropertyChanged`, `ObservableObject` and `BindableProperty` all provide a mechanism for two-way data binding between UI elements and code-behind. However, `BindableProperty` is specifically designed for use with the latest .NET MAUI framework and provides some benefits over `INotifyPropertyChanged` and `ObservableObject`, such as better performance, type safety, and compatibility with Xamarin.Forms and .NET MAUI platforms.

Additionally, `BindableProperty` allows for better control over binding modes and default values, making it a more robust and convenient solution for modern app development.

Both `ObservableObject` and `BindableObject` are required as base classes if you want to implement property change notification. It may not always be desirable or practical to modify the inheritance hierarchy of your model classes to implement data binding.

So, how can we implement the ideal solution?

The answer is to remove any base class dependencies from our model, and instead implement the`BindableProperty` directly in our component.

### Using BindableProperty in a Component

We're going to make `Person` a `BindableProperty` of our component. The actual data can be set from the component host, in this case *MainPage.Xaml.cs*. 

To begin, let's clean up our *Person.cs* model by removing the `BindableObject` base class, resulting in a plain old CLR object (POCO). Replace the contents of *Person.cs* with the following code:

*Person.cs*:

```csharp
namespace BindablePropertiesInMaui;

public class Person
{
    public Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string firstName;
    public string FirstName
    {
        get => firstName;
        set
        {
            firstName = value;
        }
    }

    public string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
        }
    }

    public DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
        }
    }
}
```

I left the setters in there so you can test them with breakpoints.

Modify the *PersonComponent.xaml* file as shown below:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
			 x:Class="BindablePropertiesInMaui.PersonComponent">

	<StackLayout MaximumWidthRequest="400">
		<Label Text="Person"
			   FontSize="Medium"
			   Margin="0,24,0,0" />
		<Entry Text="{Binding Person.FirstName}"
			   Placeholder="First Name"
			   Margin="0,24,0,0" />
		<Entry Text="{Binding Person.LastName}"
			   Placeholder="Last Name"
			   Margin="0,24,0,0" />
		<DatePicker Date="{Binding Person.DateOfBirth}"
					Margin="0,24,0,0" />
	</StackLayout>
</ContentView>
```

The only difference is the binding syntax. We now specify `Binding Person.FirstName` rather than `Binding FirstName`, etc.

Update the *PersonComponent.xaml.cs* file with the code below:

```csharp
namespace BindablePropertiesInMaui;

public partial class PersonComponent : ContentView
{
    public PersonComponent()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public static readonly BindableProperty PersonProperty = BindableProperty.Create(
        nameof(Person),
        typeof(Person),
        typeof(PersonComponent));

    public Person Person
    {
        get { return (Person)GetValue(PersonProperty); }
        set { SetValue(PersonProperty, value); }
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        Person.DateOfBirth = e.NewDate;
    }
}
```

The only thing left to do is to initialize the Person data in `MainPage`. 

Change *MainPage.xaml* to the following:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
			 x:Class="BindablePropertiesInMaui.MainPage"
			 xmlns:local="clr-namespace:BindablePropertiesInMaui">
    <local:PersonComponent x:Name="MyPersonComponent"/>
</ContentPage>
```

All we did here is give the `PersonCompoent` the name `MyPersonComponent` so we can access the `Person` property from the code behind.

Change *MainPage.xaml.cs* to the following:

```c#
namespace BindablePropertiesInMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        MyPersonComponent.Person = new Person("Carl", "Franklin", DateTime.Now);
    }
}
```

Here we are simply setting the `Person` property.

After making these changes, run the application once again and observe that the fields are still properly bound to the UI:

![Properties bound to the UI](images/a8f80aa800fec006991f68696ab8a65bd477944eba18ea825c3e98a2a3c878c3.png)  

Our model still gets updated despite not extending the `ObservableObject` class, as we have effectively implemented data binding through the use of a `BindableProperty` on the `PersonComponent`, and our model stays pure.

![ObservableObject on an Object](images/2ec1faad345ec9160b2697ca7b3e39b2237f50256beb42cddbb68a56920a31ac.png)  

## Summary

In this tutorial, we've learned about bindable properties in MAUI and how to create them. We started by implementing `INotifyPropertyChanged` to notify the UI when a property changes.

We showed how to use `ObservableObject` to reduce the boilerplate code of using `INotifyPropertyChanged`.

Then, we created a bindable property using `BindableProperty.Create` and saw how to bind to it in our UI.

Finally, we looked at how using a `BindableProperty` on the compoent, rather than implementing in our model, is the ideal solution for component properties, taking the burden of property change notification out of the model.

For more detailed information on how to develop .NET MAUI applications, check out the links to documentation and tutorials in the resources section below.

## Complete Code

The complete code for this demo, including implementation of `INotifyPropertyChanged` and `BindableProperty` with validation, can be found in the link below. You can use it as a reference to implement bindable properties in your own .NET MAUI applications.

- <https://github.com/carlfranklin/BindablePropertiesInMaui>

## Resources

Here are some helpful resources for learning more about .NET MAUI and bindable properties in MAUI:

| Resource                                 | Url                                                                               |
| ---------------------------------------- | --------------------------------------------------------------------------------- |
| The .NET Show with Carl Franklin         | <https://www.youtube.com/playlist?list=PL8h4jt35t1wgW_PqzZ9USrHvvnk8JMQy_>        |
| Download .NET                            | <https://dotnet.microsoft.com/en-us/download>                                     |
| .NET Multi-platform App UI documentation | <https://docs.microsoft.com/dotnet/maui/>                                         |
| MVVM Toolkit                             | <https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/>                 |
| MVVM Toolkit - ObservableObject          | <https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/observableobject> |
