using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadAlert.DTOs;

public record UpdateMessageStatus(
    Guid id,
    bool status);

