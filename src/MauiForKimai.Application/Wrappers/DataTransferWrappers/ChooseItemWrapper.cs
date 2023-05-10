
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public class ChooseItemWrapper
{
    public ChooseItemWrapper(IChooseItem item )
    {
        ChooseItem = item;
    }
    public IChooseItem  ChooseItem { get; set; }

    public int ChosenCustomerId {get; set; }
    public int ChosenProjectId {get; set; }

}
