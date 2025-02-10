using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos.HelperModels;

public class AuthRequest
{
    public string ClientID { get; internal set; }
    public string ClientSecret { get; internal set; }
}
