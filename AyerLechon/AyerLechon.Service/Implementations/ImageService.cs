using AyerLechon.Repo.Domains;
using System.Linq;

namespace AyerLechon.Service.Implementations
{
    public class ImageService
    {
        private AyerLechonContext _context;
        public ImageService(AyerLechonContext context)
        {
            _context = context;
        }
        public FileStorage Get(int id)
        {
            return _context.FileStorages.FirstOrDefault(a => a.FileStorageId == id);
        }
    }
}
