using MTRPZ4.Models;
using MTRPZ4.Repository.IRepository;

namespace MTRPZ4.Repository;

public class DeviceRepository : IDeviceRepository
{
    private readonly IDataStorage _data;

    public DeviceRepository(IDataStorage data)
    {
        _data = data;
    }

    public async Task<Device> GetByToken(string token) =>  _data.GetDevices().Find(x => string.Equals(x.Token, token));
    public async Task<IEnumerable<Device>> GetAll() => _data.GetDevices();
    public async Task Add(Device device) => _data.GetDevices().Add(device);
    public async Task Update(Device device)
    {
        var result = _data.GetDevices().FirstOrDefault(x => x.Token == device.Token);
        if (result is {})
        {
            result = device;
        }
    }
}