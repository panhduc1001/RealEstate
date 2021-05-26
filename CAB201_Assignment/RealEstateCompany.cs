using System.Collections.Generic;
using System.Linq;


namespace CAB201_Assignment
{
    class RealEstateCompany
    {
        private Menu mainMenu;
        private Menu functionMenu;
        private List<Customer> customers;
        private List<Property> properties;
        private Customer LoggedInUser;
        // LoggedIn property tells whether there is a customer currently using the system
        private bool LoggedIn
        {
            get
            {
                return (LoggedInUser != null);
            }
        }

        public RealEstateCompany()
        {
            mainMenu = new Menu();
            functionMenu = new Menu();
            customers = new List<Customer>();
            properties = new List<Property>();
        }

        //entry point of application - creates the realestate company and runs it.
        static void Main(string[] args)
        {
            UserInterface.Message("CAB201 - OOD Assignment - Charlie Turner n10752846");
            RealEstateCompany company = new RealEstateCompany();
            company.Run();
        }
        private void Run()
        {
            PopulateMenus();
            DisplayMenu();
        }
        private void PopulateMenus()
        {
            //populate the main menu
            mainMenu.Add("Register as a customer", RegisterCustomer);
            mainMenu.Add("Login as an existing customer", LogIn);
            mainMenu.Add("Exit", Exit);
            //populate the user's logged in option screen
            functionMenu.Add("Advertise Land", NewLand);
            functionMenu.Add("Advertise House", NewHouse);
            functionMenu.Add("List My Properties", ListUsersProperties);
            functionMenu.Add("Search Properties", SearchProperties);
            functionMenu.Add("Bid on a property", PlaceBid);
            functionMenu.Add("List bids received for a property", ListBidsForProperty);
            functionMenu.Add("Sell one of your properties to the highest bidder", SellProperty);
            functionMenu.Add("Logout", Logout);
            //populate 
        }

        private void DisplayMenu()
        {
            while (true)
            {
                if (LoggedIn)
                {
                    functionMenu.Display();
                }
                else
                {
                    mainMenu.Display();
                }
            }
        }


        // Actions - for menus
        // -- Main Menu
        private void RegisterCustomer()
        {
            string pwd = "";
            string firstName = UserInterface.GetInput("First name");
            string lastName = UserInterface.GetInput("Last name");
            string email;
            // ensure email isn't already taken
            email = UserInterface.GetInput("Email address");
            if (customers.Find(cust => cust.Email == email) != null)
            {
                UserInterface.Error("Email taken");
                return;
            }
            bool pwd_confirmed = false;
            // Confirm password
            while (!pwd_confirmed)
            {
                pwd = UserInterface.GetPassword("Password");
                string pwd_ = UserInterface.GetPassword("Confirm Password");
                if (pwd == pwd_)
                {
                    pwd_confirmed = true;
                }
                else
                {
                    UserInterface.Error("Sorry, those passwords don't match");
                }
            }
            customers.Add(new Customer(firstName, lastName, email, pwd));
            UserInterface.Message(firstName + " " + lastName + " registered successfully");
        }

        private void LogIn()
        {
            string emailInp = UserInterface.GetInput("Email");
            string pwdInp = UserInterface.GetPassword("Password");
            Customer customer = customers.Find(r => r.Email == emailInp);
            if (customer == null) // error if customer with corresponding email doesn't exist
            {
                UserInterface.Error("Email not found");
            }
            else
            {
                //if the customer does exist, let the customer authenticate themselves. Will error and return null if password/email combination is incorrect 
                LoggedInUser = customer.Authenticate(emailInp, pwdInp);
            }
        }

        private void Exit() { System.Environment.Exit(0); }
        // -- Actions for user logged in menu
        private void NewLand() { properties.Add(LoggedInUser.ListNewProperty(1)); }
        private void NewHouse() { properties.Add(LoggedInUser.ListNewProperty(2)); }


        private void ListUsersProperties()
        {
            List<Property> filtered_listings = Property.FilterProperties(LoggedInUser, properties);
            UserInterface.DisplayList("All of your properties", filtered_listings, "You do not currently have any properties listed");
        }

        private void SearchProperties()
        {
            string postcode = UserInterface.GetInput("Enter postcode for search");
            List<Property> filtered_listings = Property.FilterProperties(postcode, properties);
            UserInterface.DisplayList($"All properties in postcode {postcode}", filtered_listings, "No properties are for sale in your chosen postcode");
        }
        private void PlaceBid()
        {
            UserInterface.Message("Select a property to bid on");
            // We don't need to show the customer their own properties to bid on.
            // filteredproperties is evaluated by taking all properties except ones belonging to customer, then parsing it back as a list of properties
            List<Property> filteredProperties = properties.Except(Property.FilterProperties(LoggedInUser, properties)).ToList();
            if (filteredProperties.Count == 0)
            {
                UserInterface.Error("No properties available to bid on");
                return;
            }
            Property selected = UserInterface.ChooseFromList(filteredProperties);
            // tell the customer to place a bid, then display the output to the screen
            UserInterface.Message(LoggedInUser.PlaceBid(selected));
        }

        private void ListBidsForProperty()
        {
            Property selected = UserInterface.ChooseFromList(Property.FilterProperties(LoggedInUser, properties));
            selected.ListBids();
        }

        private void SellProperty()
        {
            // Realestate company enquires about which property the customer wishes to sell, then tells the customer object to sell the selected property
            UserInterface.Message("Which property would you like to sell?");
            Property selected = UserInterface.ChooseFromList(Property.FilterProperties(LoggedInUser, properties));
            if(selected != null)
            {
                LoggedInUser.SellProperty(selected);
            }           
        }

        private void Logout()
        {
            LoggedInUser = null;
        }
    }
}
