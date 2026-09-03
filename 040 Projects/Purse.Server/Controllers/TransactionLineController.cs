using CommunityToolkit.Datasync.Server;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Purse.Server.Data;
using Purse.Server.Model;

namespace Purse.Server.Controllers
{
    [Route("tables/transactionline")]
    public class TransactionLineController : TableController<TransactionLineItem>
    {
        public TransactionLineController(AppDbContext context)
            : base(new EntityTableRepository<TransactionLineItem>(context))
        {
        }
    }
}