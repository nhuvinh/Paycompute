using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Paycompute.Entity;

namespace Paycompute.Persitence
{
				public class ApplicationDbContext : IdentityDbContext
				{
								public DbSet<PaymentRecord> PaymentRecords { get; set; }
								public DbSet<Employee> Employees { get; set; }
								public DbSet<TaxYear> TaxYears { get; set; }
								public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
												: base(options)
								{
								}
				}
}
