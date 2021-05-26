using System;
using System.Collections.Generic;
using System.Linq;

namespace CAB201_Assignment
{
    public abstract class Property
    {
        // Fields and Properties
        protected Customer owner;
        public Customer Owner { get { return owner; } }
        protected string address;
        protected string Postcode;
        protected List<Bid> Bids { get; set; }
        // Methods
        public Property(Customer owner, string address, string postcode)
        {
            this.owner = owner;
            this.address = address;
            this.Postcode = postcode;
            Bids = new List<Bid>();
        }
        public static List<Property> FilterProperties(Customer owner, List<Property> properties)     // overloaded method with parameter customer
        {
            List<Property> _properties = new List<Property>();

            foreach (Property prop in properties)
            {
                if (prop.owner == owner) _properties.Add(prop);
            }
            return _properties;
        }
        public static List<Property> FilterProperties(string postcode, List<Property> properties)    // overloaded method with parameter postcode string
        {
            List<Property> _properties = new List<Property>();

            foreach (Property prop in properties)
            {
                if (prop.Postcode == postcode) _properties.Add(prop);
            }
            return _properties;
        }
        public abstract void ListBids();
        public abstract int SalesTax(int salePrice);

        public int GetHighestBidAmount()
        {
            // ensure the property has bids
            if (Bids.Count > 0)
            {
                return Bids.Max(r => r.BidAmount);
            }
            else
            {
                // no bids for current property
                return 0;
            }
        }

        public void AddBid(Customer customer, int bidAmount)
        {
            Bids.Add(new Bid(customer, bidAmount));
        }

        public void TransferToHighestBidder()
        {
            Bid highestBid = Bids.Find(x => x.BidAmount == GetHighestBidAmount());
            Customer newOwner = highestBid.Bidder;
            owner = newOwner;
            // bids can now be reset, the property has been transfered
            Bids = new List<Bid>();
        }

    }

    class Land : Property
    {
        public int Size { get; set; }

        public Land(Customer owner, string address, string postcode, int size) : base(owner, address, postcode)
        {
            this.Size = size;
        }

        public override void ListBids()
        {
            UserInterface.DisplayList($"All bids for {this.address} (Land Only)", Bids, "No bids for this land have been received");
        }

        public override string ToString()
        {
            return $"{address}, {Postcode}, Land: {Size} square metres";
        }

        public override int SalesTax(int salePrice)
        {
            // sales tax is equal to $5.50 per square metre, rounded to nearest whole dollar amount
            return (int)Math.Round(Size * 5.5);

        }
    }

    class House : Property
    {
        public string description { get; set; }

        public House(Customer owner, string address, string postcode, string description) : base(owner, address, postcode)
        {
            this.description = description;
        }
        public override void ListBids()
        {
            UserInterface.DisplayList($"All bids for {this.address} (Land and House)", Bids, "No bids for this house have been received");
        }
        public override string ToString()
        {
            return $"{address}, {Postcode}, House: {description}";
        }

        public override int SalesTax(int salePrice)
        {
            // sales tax for houses is equal to 10% of the sale price, rounded to the nearest dollar
            return (int)Math.Round(salePrice * 0.1);
        }
    }


}
