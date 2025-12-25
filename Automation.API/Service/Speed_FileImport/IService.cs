using Network.API.ViewModel.Speed_ThongTinNhanVienDo;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Speed_FileImport
{
    public interface IService: IRepositoryBase<Model.Speed_FileImport>
    {
        Task<bool> IsExistsFileImport(string fileName);
        Task<string> LastFileImportAsync();
    }
}
