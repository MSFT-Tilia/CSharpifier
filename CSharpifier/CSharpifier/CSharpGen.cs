using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CSharpifier
{
    public class CSharpGen
    {
        public void Generate(string filename, CSFileNode fileNode, CSharpEmitterBase emitter)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs);
                Generate(sw, fileNode, emitter);
                fs.Close();
            }
        }

        public void Generate(StreamWriter ostream, CSFileNode fileNode, CSharpEmitterBase emitter)
        {
            emitter.OutputStream = ostream;
            _curEmitter = emitter;
            _curEmitter.Visit(fileNode);
            ostream.Flush();
        }

        private CSharpEmitterBase _curEmitter;
    }
}
