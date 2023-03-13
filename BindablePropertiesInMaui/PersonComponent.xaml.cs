namespace BindablePropertiesInMaui;

public partial class PersonComponent : ContentView
{
    private PersonEditor personEditor;

    // This person obhect would typically come from a db or another store
    readonly Person person = new("Carl", "Franklin", DateTime.Now);

    public PersonComponent()
    {
        InitializeComponent();
        personEditor = new PersonEditor(person);
        BindingContext = personEditor;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        personEditor.Person.DateOfBirth = e.NewDate;
    }
}