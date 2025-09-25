using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public Address ShippedTo { get; set; }
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }

    public class OrderBuilder
    {
        private readonly Order _order = new();

        private OrderBuilder() { }

        public static OrderBuilder Create()
        {
            return new OrderBuilder();
        }

        public OrderBuilder Id(int id)
        {
            _order.Id = id;
            return this;
        }

        public OrderBuilder Price(decimal price)
        {
            _order.Price = price;
            return this;
        }

        public OrderBuilder ShippedTo(Action<AddressBuilder> addressBuilderAction)
        {
            var addressBuilder = new AddressBuilder();
            addressBuilderAction(addressBuilder);
            _order.ShippedTo = addressBuilder.Build();
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }

    public class AddressBuilder
    {
        private readonly Address _address = new();

        public AddressBuilder Street(string street)
        {
            _address.Street = street;
            return this;
        }

        public AddressBuilder City(string city)
        {
            _address.City = city;
            return this;
        }

        public AddressBuilder State(string state)
        {
            _address.State = state;
            return this;
        }

        public AddressBuilder Zip(string zip)
        {
            _address.Zip = zip;
            return this;
        }

        public Address Build()
        {
            return _address;
        }
    }
}