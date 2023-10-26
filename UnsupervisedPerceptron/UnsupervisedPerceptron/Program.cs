namespace UnsupervisedPerceptron
{
    internal class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            int NUMBER_OF_NEURONS = 10;
            int NUMBER_OF_TRAINING_CYCLES = 20;

            double in1 = Math.Round(rnd.NextDouble(), 2);
            double in2 = Math.Round(rnd.NextDouble(), 2);

            List<KohonenNeuron> neurons = new List<KohonenNeuron>();
            for (int i = 0; i < NUMBER_OF_NEURONS; i++) // initalizing neurons
            {
                neurons.Add(new KohonenNeuron(in1,in2, i)); 
            }

            Console.WriteLine($"Desired Value: {neurons[0].InputVectorLength}, Input1: {in1}, Input2: {in2}");

            double minDeg = neurons.Min(y => y.DegreeOfInputVector_and_WeightVector); // selecting the vector closest (smallest degree value between it and the input vector)
                                                                                      // to the input vector
            KohonenNeuron winnerNeuron = neurons[neurons.IndexOf(neurons.FirstOrDefault(x => x.DegreeOfInputVector_and_WeightVector == minDeg))];

            for (int i = 0; i < NUMBER_OF_TRAINING_CYCLES; i++) // running training loop
            {
                winnerNeuron.TrainWeights();
                winnerNeuron.StartEval();
                Console.WriteLine($"\tNeuron: {winnerNeuron.ID} output: {winnerNeuron.WeightVectorLength}, weight_1: {winnerNeuron.Weight_1}, weight_2: {winnerNeuron.Weight_2} Degree between InputVector: {winnerNeuron.DegreeOfInputVector_and_WeightVector}°");
            }
        }
    }
    public class KohonenNeuron
    {
        private static Random rnd = new Random();
        public double Weight_1 { get; private set; }
        public double Weight_2 { get; private set; }
        private double Input_1;
        private double Input_2; 
        private double Normalized_Input_1;
        private double Normalized_Input_2;
        private double Normalized_Weight_1;
        private double Normalized_Weight_2;
        private double LearningRestraint = 0.3; // greater learning restraint, less training time, and vice-versa
        public double InputVectorLength { get; private set; }
        public double WeightVectorLength { get; private set; }
        public double DegreeOfInputVector_and_WeightVector { get; private set; }
        public double Output { get; private set; }
        public int ID { get; private set; }
        public KohonenNeuron(double input1, double input2, int ID)
        {
            this.ID = ID;
            this.Input_1 = input1; // handle the two inputs as coordinates
            this.Input_2 = input2;

            this.InputVectorLength = Math.Round(Math.Sqrt(Math.Pow(Input_1, 2) + Math.Pow(Input_2, 2)), 4); // calculating length of the input vector
            this.Normalized_Input_1 = Math.Round(Input_1 / InputVectorLength, 4); // normalizing inputs
            this.Normalized_Input_2 = Math.Round(Input_2 / InputVectorLength, 4);

            this.Weight_1 = rnd.NextDouble() * 2 - 1; // initializing random values to the weights
            this.Weight_2 = rnd.NextDouble() * 2 - 1;
        }
        public void StartEval(double? input1 = null, double? input2 = null)
        {
            if (input1 != null && input2 != null) // if there are new input values, then we re-calculate the length, and then re-normalize the vectors
            {
                this.Input_1 = input1 ?? 0.0;
                this.Input_2 = input2 ?? 0.0;

                this.InputVectorLength = Math.Round(Math.Sqrt(Math.Pow(Input_1, 2) + Math.Pow(Input_2, 2)), 4);
                this.Normalized_Input_1 = Math.Round(Input_1 / InputVectorLength, 4);
                this.Normalized_Input_2 = Math.Round(Input_2 / InputVectorLength, 4);
            }

            

            this.Weight_1 = Math.Round(Weight_1, 2); // rounding weights down to 2 deciamls
            this.Weight_2 = Math.Round(Weight_2, 2);
            this.WeightVectorLength = Math.Round(Math.Sqrt(Math.Pow((double)Weight_1, 2) + Math.Pow((double)Weight_2, 2)), 4); // calculating the length of the weight vector(s)

            this.Normalized_Weight_1 = Math.Round((double)Weight_1 / WeightVectorLength, 4); // normalizing weight vectors
            this.Normalized_Weight_2 = Math.Round((double)Weight_2 / WeightVectorLength, 4);


            this.Output = (this.Normalized_Input_1 * this.Normalized_Weight_1) + (this.Normalized_Input_2 * this.Normalized_Weight_2);

            GetDegree();
        }
        public void TrainWeights()
        {
            this.Weight_1 = Weight_1 + LearningRestraint * (this.Input_1 - this.Weight_1); // adjusting weights
            this.Weight_2 = Weight_2 + LearningRestraint * (this.Input_2 - this.Weight_2);
        }
        private void GetDegree()
        {
            double Left = this.Output; // calculating degree between the input vector and the weight vector
            this.DegreeOfInputVector_and_WeightVector = Math.Round((Math.Acos(Left) * 57.296), 2);
        }
    }
}