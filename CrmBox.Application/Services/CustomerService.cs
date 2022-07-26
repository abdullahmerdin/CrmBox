using CrmBox.Application.Interfaces;
using CrmBox.Core.Domain;
using CrmBox.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace CrmBox.Application.Services;

    public class CustomerService : GenericService<Customer, CrmBoxContext>, ICustomerService
{
        readonly CrmBoxContext _context;
        public CustomerService(CrmBoxContext context) : base(context)
        {
            _context = context;
        }
        
    }
