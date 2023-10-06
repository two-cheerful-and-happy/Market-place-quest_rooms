 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels.OwnerOfRoom;

public class LocationOfUserVIewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public byte[] Photo { get; set; }
    public Mark Mark { get; set; }
}
