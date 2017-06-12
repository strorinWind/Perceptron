using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perceptron.UI
{
    public class NetworkBad
    {
        private ActivationNetwork network;

        public NetworkBad(int n)
        {
            network = new ActivationNetwork(
            new SigmoidFunction(), // sigmoid activation function
            2,                      // 3 inputs
            n,
            2);
        }


        public void Learn(double[][] input,double[][] output)
        {
            
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            double error = 1;
            int k=input.Length;
            double prerr = 0;
            while (k>=input.Length/100)
            {
                // run epoch of learning procedure

                error = teacher.RunEpoch(input, output);

                prerr = error;

                k = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    var c = network.Compute(input[i]);
                    if (c[0]>c[1] & output[i][0]<output[i][1] || c[0] < c[1] & output[i][0] > output[i][1])
                    {
                        k++;
                    }
                    /*
                    if (Math.Round(c[0]) != output[i][0])
                    {
                        
                    }*/                  
                }
                Console.WriteLine(k +" " +error + " "+teacher.LearningRate);
                //c = network.Compute(input[1000]);
                // check error value to see if we need to stop
                // ...
            }
        }
    }
}
