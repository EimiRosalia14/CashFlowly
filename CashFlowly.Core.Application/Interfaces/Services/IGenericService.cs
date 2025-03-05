namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IGenericService<CreateDTO, UpdateDTO, Entity, Response>
        where CreateDTO : class
        where UpdateDTO : class
        where Entity : class
        where Response : class
    {
        public Task<Response> FindByIdAsync(int id);
        public Task<List<Response>> FindAllAsync();
        public Task<Response> CreateAsync(CreateDTO createDTO);
        public Task<Response> UpdateAsync(UpdateDTO updateDTO, int id);
        public Task DeleteAsync(int id);
    }
}
