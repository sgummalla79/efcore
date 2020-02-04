// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Microsoft.EntityFrameworkCore.Metadata
{
    public class RelationalModelTest
    {
        [ConditionalFact]
        public void Can_use_relational_model()
        {
            var modelBuilder = CreateConventionModelBuilder();

            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<Order>().OwnsOne(o => o.Details);

            var model = modelBuilder.FinalizeModel();

            foreach(var entityType in model.GetEntityTypes())
            {
                Assert.Single(entityType.GetTableMappings());
            }
        }

        protected virtual ModelBuilder CreateConventionModelBuilder() => RelationalTestHelpers.Instance.CreateConventionBuilder();

        private enum MyEnum : ulong
        {
            Sun,
            Mon,
            Tue
        }

        private class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public short SomeShort { get; set; }
            public MyEnum EnumValue { get; set; }

            public IEnumerable<Order> Orders { get; set; }
        }

        private class SpecialCustomer : Customer
        {
            public string Speciality { get; set; }
        }

        private class Order
        {
            public int OrderId { get; set; }

            public int CustomerId { get; set; }
            public Customer Customer { get; set; }

            public OrderDetails Details { get; set; }
        }

        private class OrderDetails
        {
            public int OrderId { get; set; }
            public Order Order { get; set; }
        }
    }
}
