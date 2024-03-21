using MdxLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeng.MdxImport.mdx.handlers
{
    public class LayerHandler
    {
        public int index;
        public CMaterialLayer clayer;
        public MaterailHandler materailHandler;


        public LayerHandler(int index, CMaterialLayer clayer, MaterailHandler materailHandler)
        {
            this.index = index;
            this.clayer = clayer;
            this.materailHandler = materailHandler;
        }

    }
}
