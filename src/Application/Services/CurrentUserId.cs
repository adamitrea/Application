using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICurrentUserId
    {
        int getID();
    }
    public class CurrentUserId: ICurrentUserId
    {
        public int getID()
        {
            return 1;
        }
    }
}
