using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class arduControl : MonoBehaviour {

	private SerialPort	sp = new SerialPort();
	Quaternion			gyroRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
	
	    sp.BaudRate = 115200;
        
        sp.PortName = "/dev/cu.wchusbserialfd130";
        sp.DataBits = 8;
        sp.StopBits = StopBits.One;
        sp.Parity = Parity.None;
        sp.Handshake = Handshake.RequestToSend;

		sp.Open();
		if (!sp.IsOpen)
		{
			Debug.Log("failed to open perif");
			return ;
		}
	    sp.ReadTimeout = 100;
    	sp.WriteTimeout = 100;
		sp.WriteLine("r");
		sp.DtrEnable = true;
		sp.RtsEnable = true;
		Debug.Log("SerialPort binded !");
		Debug.Log(sp.ReadLine());

		sp.DataReceived += new SerialDataReceivedEventHandler(gyro_event); 
	}

	private void gyro_event(object sender, SerialDataReceivedEventArgs e) 
    { 
    	string line = sp.ReadExisting(); 
		Debug.Log("received line: " + line);
		/*gyroRotation[0] = ((line[2] << 8) | line[3]) / 16384.0f;
		gyroRotation[1] = ((line[4] << 8) | line[5]) / 16384.0f;
		gyroRotation[2] = ((line[6] << 8) | line[7]) / 16384.0f;
		gyroRotation[3] = ((line[8] << 8) | line[9]) / 16384.0f;
		for (int i = 0; i < 4; i++) if (gyroRotation[i] >= 2) gyroRotation[i] = -4 + gyroRotation[i];*/
    } 
	
	// Update is called once per frame
	void Update () {
		transform.rotation = gyroRotation;
	}
}
