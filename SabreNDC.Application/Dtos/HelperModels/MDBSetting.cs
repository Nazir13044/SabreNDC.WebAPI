using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos.HelperModels;

public class MDBSetting
{
    public string Connection { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
