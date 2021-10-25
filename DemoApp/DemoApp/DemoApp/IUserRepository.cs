using DemoApp.Models;
using DemoApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp
{
    public interface IUserRepository
    {
        UserDTO GetUser(UserModel userModel);
    }
}
