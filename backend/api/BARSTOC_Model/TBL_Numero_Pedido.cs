using System;
using System.Collections.Generic;

namespace BARSTOC_Model;

public partial class TBL_Numero_Pedido
{
    public int IdNumeroPedido { get; set; }

    public int UltimoNumero { get; set; }

    public string? Prefijo { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
