﻿using ManufacturingInventory.Infrastructure.Model.Entities;

namespace ManufacturingInventory.Domain.Security.Interfaces {
    public interface IUserServiceProvider {
        IUserService CreateUserServiceUserAuthenticated(User user, bool storePassword, string permission = null, string password = null);
        IUserService CreateUserServiceNoPermissions(User user, bool storePassword, string password = null);
    }
}
