﻿namespace ComplexValidation.Configuration.Model.InMemory
{
    using System;
    using System.Collections.Generic;
    using RealPersistence;

    public class InMemoryCustomerRepository : ICustomerRepository
    {
        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            return new[]
            {
                new Customer { Id = 1, Name = "Cliente 1"},
                new Customer { Id = 2, Name = "Cliente 2"},
                new Customer { Id = 3, Name = "Cliente 3"},
                new Customer { Id = 4, Name = "Cliente 4"},
                new Customer { Id = 5, Name = "Cliente 5"},
                new Customer { Id = 6, Name = "Cliente 6"},
            };
        }
    }
}