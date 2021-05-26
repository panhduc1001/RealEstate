
namespace CAB201_Assignment
{
    public class Customer
    {
        // Auto-property fields 
        // email must have a public getter so that the company can check each new user has a unique email
        private string email;
        public string Email { get { return email; } }
        // Customer records/details only able to be accessed by the customer themselves
        private string firstName;
        private string lastName;
        private Password password;

        public string FullName { get { return firstName + " " + lastName; } }

        public Customer(string firstName, string lastName, string email, string password)
        {

            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;

            // we never want to store a user's plain text password. Password class deals with all salting/hashing
            this.password = new Password(password);

        }

        public Customer Authenticate(string emailInp, string pwdInp)
        {
            bool pwdMatch = password.checkPassword(pwdInp);
            bool emailMatch = (emailInp == email);
            if (emailMatch && pwdMatch)
            {
                UserInterface.Message($"Logged in as {this.FullName}");
                return this;
            }
            else
            {
                UserInterface.Error("Password incorrect");
                return null;
            }
        }

        public Property ListNewProperty(int type)
        {
            string address = UserInterface.GetInput("Address");
            string postcode = UserInterface.GetInput("Postcode");
            // Switch statement allows for extension to other property types in future
            switch (type)
            {
                case 1: // Land
                    int size = UserInterface.GetInteger("Size (square metres)");
                    return new Land(this, address, postcode, size);
                case 2: // House
                    string description = UserInterface.GetInput("Description of house");
                    return new House(this, address, postcode, description);
                default: // This code should never be reached
                    UserInterface.Error("Something went wrong");
                    return null;
            }
        }


        public string PlaceBid(Property property)
        {
            int min = property.GetHighestBidAmount();
            int bidAmount = UserInterface.GetInteger($"Input bid amount (current highest bid: ${min})");
            if (bidAmount <= min) return "Bid cannot be placed as it is less than the minimum amount";
            property.AddBid(this, bidAmount);
            return $"Successfully placed ${bidAmount} bid on {property}";

        }
        public void SellProperty(Property property)
        {
            // find bid with the highest bid amount
            int highestAmount = property.GetHighestBidAmount();

            // if there are no bids (GetHighestBid returned 0), error and return
            if (highestAmount == 0)
            {
                UserInterface.Error("There are no bids for this property at the current time");
                return;
            }
            // If there are bids, transfer the property         
            property.TransferToHighestBidder();
            UserInterface.Message($"Successfully sold {this} to {property.Owner}");
            UserInterface.Message($"Applicable sales tax: ${property.SalesTax(highestAmount)}");


        }
        public override string ToString()
        {
            return $"{FullName} ({email})";
        }
    }
}
