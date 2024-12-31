using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities2Dto;

internal class UserService : IUserRepository<FullUtilisateurDto, FullUtilisateurDto>
{

    private IUserRepository<UtilisateurEntities> Repository { get; set; }

    public UserService(IUserRepository<UtilisateurEntities> repository)
    {
        Repository = repository;
    }
    public async Task<bool> Delete(string id)
        => await Repository.Delete(id);

    public async Task<Pagination<FullUtilisateurDto>> GetAllUser(UserOrderingCriteria criteria = UserOrderingCriteria.None, int index = 0, int count = 10)
        => (await Repository.GetAllUser(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullUtilisateurDto?> GetById(string id)
        => (await Repository.GetById(id))?.ToDto();

    public async Task<Pagination<FullUtilisateurDto>> GetUserById(UserOrderingCriteria criteria = UserOrderingCriteria.Id, int index = 0, int count = 5)
        => (await Repository.GetUserById(criteria, index, count)).ToPagingResponseDtos();

    public async Task<Pagination<FullUtilisateurDto>> GetUserMail(UserOrderingCriteria criteria = UserOrderingCriteria.Mail, int index = 0, int count = 5)
        => (await Repository.GetUserMail(criteria, index, count)).ToPagingResponseDtos();

    public async Task<FullUtilisateurDto?> Insert(FullUtilisateurDto item)
        => (await Repository.Insert(item.ToEntities()))?.ToDto();

    public async Task<FullUtilisateurDto?> Update(string id, FullUtilisateurDto item)
        => (await Repository.Update(id, item.ToEntities()))?.ToDto();
}
