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