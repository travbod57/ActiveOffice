using DAL;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveOffice
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IClass1>().To<Class1>();
        }
    }
}
