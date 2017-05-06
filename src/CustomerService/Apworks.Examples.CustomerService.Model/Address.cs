using System;

namespace Apworks.Examples.CustomerService.Model
{
    /// <summary>
    /// Represents the address value object.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }
    }
}
