using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace mkvep
{
    class Program
    {
        const int headersize = 270;
        static void Main(string[] args)
        {
            FileStream stream = File.Open(args[1], FileMode.Open);
            Console.WriteLine("0x" + ((int)stream.Length).ToString("x"));
            switch (args[0])
            {
                // extract binary data from VEP
                case "r":
                    // seek to the data
                    stream.Seek(headersize, SeekOrigin.Begin);
                    // create byte array
                    byte[] data = new byte[stream.Length];
                    Console.WriteLine("Bytes of data to extract: 0x" + ((int)stream.Length).ToString("x"));
                    // read
                    stream.Read(data, 0, (int)stream.Length);
                    // write
                    File.WriteAllBytes(args[1] + ".bin", data);
                    Console.WriteLine("Done");
                    break;
                // write binary data to VEP
                case "w":
                    // close and read the binary data
                    stream.Close();
                    byte[] progdata = File.ReadAllBytes(args[1]);
                    string outfile = args[2];
                    // we store the header here
                    byte[] dt = BitConverter.GetBytes(progdata.Length);
                    byte[] header = { (byte)'V', (byte)'P', 0, 0, 1, 0, 0, dt[0], dt[1], dt[2], dt[3], 0, 0, 0, 0};
                    // this is the whole 270 byte array
                    byte[] fheader = new byte[headersize];
                    // copy it
                    Array.Copy(header, fheader, header.Length);
                    // outdata is the data to write
                    byte[] outdata = new byte[fheader.Length + progdata.Length];
                    // copy both arrays
                    Buffer.BlockCopy(fheader, 0, outdata, 0, fheader.Length);
                    Buffer.BlockCopy(progdata, 0, outdata, fheader.Length, progdata.Length);
                    File.WriteAllBytes(outfile,outdata);
                    Console.WriteLine("Done, wrote to " + outfile);
                    break;
            }
            Console.WriteLine("Finished");
            // exit successfully 
            Environment.Exit(0);
        }
    }
}
