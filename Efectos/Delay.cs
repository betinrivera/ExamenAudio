using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        public int offsetTiempoMS;


        List<float> muestras = new List<float>();

        private float factor;
        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                if (value > 1)
                    factor = 1;
                else if (value < 0)
                    factor = 0;
                else
                    factor = value;
            }
        }


        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            Factor = factor;
            if (factor > 1)
                Factor = 1;
            else if (factor < 0)
                Factor = 0;
        }

       public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }
       


        //Offset es el numero de muestras leidas hasta ahorita
        public int Read(float[] buffer, int offset, int count)
        {



            var read = fuente.Read(buffer, offset, count);

            float tiempoTranscurrido = 
                (float)muestras.Count / (float)fuente.WaveFormat.SampleRate;

            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = 
                tiempoTranscurrido * 1000;

            int numMuestrasOffsetTiempo = 
                (int)(((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);



            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }




            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[offset + i] +=
                       muestras[muestrasTranscurridas + i - numMuestrasOffsetTiempo] * Factor;
                }
            }


            return read;
        }
    }
}
