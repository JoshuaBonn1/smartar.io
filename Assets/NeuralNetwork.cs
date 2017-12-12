using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour  {

	public int numOfInputs = 360;
	public int numOfHidden = 360;
	public int numOfOutputs = 1;
	public float[] ins;
	private float[] weights;
	public float[] output;

	void Start () {
		weights = new float[numOfInputs * numOfHidden + numOfHidden * numOfOutputs];
		for (int w = 0; w < weights.Length; w++)
			weights [w] = Random.value * 2.0f - 1.0f;
		output = new float[numOfOutputs];
		ins = new float[numOfInputs];
	}

	public void feedForward (float[] inputs) {
		ins = inputs;
		float[] hidden = new float[numOfHidden];
		for (int i = 0; i < hidden.Length; i++)
			hidden [i] = 0.0f;

		// Input to Hidden
		for (int input = 0; input < inputs.Length; input++) {
			for (int hid = 0; hid < hidden.Length; hid++) {
				int weightIndex = input * hidden.Length + hid;
				hidden [hid] += inputs [input] * weights [weightIndex];
				//print (hidden [hid]);
			}
		}

		// Activate
		for (int hid = 0; hid < hidden.Length; hid++) {
			hidden [hid] = sigmoidFunction (hidden [hid]);

		}

		// Hidden to Output
		for (int i = 0; i < output.Length; i++)
			output [i] = 0.0f;
		for (int hid = 0; hid < hidden.Length; hid++) {
			for (int outs = 0; outs < output.Length; outs++) {
				int weightIndex = hid * output.Length + outs + numOfInputs * numOfHidden;
				output [outs] += hidden [hid] * weights [weightIndex];
			}
		}

		// Activate and set value to 0-359
		for (int outs = 0; outs < output.Length; outs++) {
			output [outs] = sigmoidFunction (output [outs]);
			output [outs] *= 360;
		}
	}

	private float sigmoidFunction (float val) {
		// print (val);
		//Debug.Log (1 / (1 + Mathf.Exp (-val)));
		return 1 / (1 + Mathf.Exp (-val));
	}
}
