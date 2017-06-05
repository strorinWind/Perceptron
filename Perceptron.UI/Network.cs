using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perceptron.UI
{
    public class Network
    {
        public void Learn(double[][] input,double[][] output)
        {
            ActivationNetwork network = new ActivationNetwork(
            new SigmoidFunction(), // sigmoid activation function
            2,                      // 3 inputs
            10,
            1);
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            double error = 1;
            while (error>0.01)
            {
                // run epoch of learning procedure
                error = teacher.RunEpoch(input, output);
                var c = network.Compute(input[0]);
                if (c[0]<0.6)
                {
                    var a = 5;
                }
                //c = network.Compute(input[1000]);
                // check error value to see if we need to stop
                // ...
            }
        }
    }
}
