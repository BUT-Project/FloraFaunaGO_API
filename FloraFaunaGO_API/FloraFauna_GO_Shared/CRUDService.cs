using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Shared;

public interface CRUDService<T>
{
    T Get(Guid id);
    bool Delete(Guid id);
    bool Update(T item);
    T Create(T item);
}
