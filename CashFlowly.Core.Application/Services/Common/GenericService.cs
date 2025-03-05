using AutoMapper;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;

namespace CashFlowly.Core.Application.Services.Common
{
    public class GenericService<CreateDTO, UpdateDTO, Entity, Response> : IGenericService<CreateDTO, UpdateDTO, Entity, Response>
        where CreateDTO : class
        where UpdateDTO : class
        where Entity : class
        where Response : class
    {
        private readonly IGenericRepository<Entity> _repo;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Response> CreateAsync(CreateDTO createDTO)
        {
            var createEntity = _mapper.Map<Entity>(createDTO);
            try
            {
                var result = await _repo.CreateAsync(createEntity);
                return _mapper.Map<Response>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Un error ha sucesido registrando la entidad {nameof(Entity)}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(result);
        }

        public async Task<List<Response>> FindAllAsync()
        {
            return _mapper.Map<List<Response>>(await _repo.GetAllAsync());
        }

        public async Task<Response> FindByIdAsync(int id)
        {
            return _mapper.Map<Response>(await _repo.GetByIdAsync(id));
        }

        public async Task<Response> UpdateAsync(UpdateDTO updateDTO, int id)
        {
            Entity result = await _repo.UpdateAsync(_mapper.Map<Entity>(updateDTO), id);
            return _mapper.Map<Response>(result);
        }
    }
}
