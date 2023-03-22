namespace BindablePropertiesInMaui;

public partial class MainPage : ContentPage
{
    Person MyPerson { get; set; }

    public MainPage()
    {
        InitializeComponent();

        // Retrieve or create a Person object
        MyPerson = new Person("Carl", "Franklin", DateTime.Now);

        // Set the Bindable Person Property on the component
        MyPersonComponent.Person = MyPerson;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Update the Person object
        MyPerson.FirstName = "Isadora";
        MyPerson.LastName = "Jarr";
        MyPerson.DateOfBirth = DateTime.Parse("April 1, 1966");

        // Tell the component to refresh
        MyPersonComponent.Refresh();
    }
}