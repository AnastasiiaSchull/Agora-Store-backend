using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;
using Agora.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Services
{
    public class AdminService : IAdminService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public AdminService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IQueryable<AdminDTO>> GetAll()
        {
            var admins = await Database.Admins.GetAll();
            return _mapper.Map<IQueryable<AdminDTO>>(admins.ToList());
        }

        public async Task<AdminDTO> Get(int id)
        {
            var admin = await Database.Admins.Get(id);
            if (admin == null)
                throw new ValidationExceptionFromService("There is no admin with this id", "");
            return new AdminDTO
            {
                Id = admin.Id,
                UserId = admin.UserId,
            };
        }

        public async Task Create(AdminDTO adminDTO)
        {
            var admin = new Admin
            {
                Id= adminDTO.Id,
                UserId = adminDTO.UserId,
            };
            await Database.Admins.Create(admin);
            await Database.Save();
        }
        public async Task Update(AdminDTO adminDTO)
        {
            var admin = new Admin
            {
                Id = adminDTO.Id,
                UserId= adminDTO.UserId,
            };
            Database.Admins.Update(admin);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Admins.Delete(id);
            await Database.Save();
        }
    }
}
