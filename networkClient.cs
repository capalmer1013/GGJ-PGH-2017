using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class networkClient : MonoBehaviour{
    // will not parse other objects
    // just players and ball
    public Vector3 player1Location;
    public float player1Theta;

	public Vector3 player2Location;
    public float player2Theta;

	public Vector3 ballLocation;
	public Vector3 ballRotation;

	public UdpClient udpReceiver;
	public IPAddress serverIp;

	public List<Vector3> player2Locations = new List<Vector3>();
	public List<Vector3> ballPositions = new List<Vector3>();
	public List<Vector3> ballRotations = new List<Vector3>();

	int locationInt = 0;

	public GameObject ball;

	public int playerIndex;

	void Start (){

		ball = GameObject.Find ("Ball");

		udpReceiver = new UdpClient (5010);

		udpReceiver.JoinMulticastGroup (IPAddress.Parse ("224.0.0.1"));

		udpReceiver.BeginReceive (DataReceived, udpReceiver);
	
		joinServer ();

	}
    
    public void joinServer(){
        // send a udp message to the server
        // ascii string "suh dude"
    }

	public void sendUpdateToServer(){
        // send the current values for these things to the server
        // on port 5009

		byte[] allTheBytes = new byte[1024];
		int totalBytes = 0;

        if(/*this is player 1*/){
            float player1Theta = transform.rotation.eulerAngles.y;
            float player2Theta = gameObject however you access the other objects theta

        } else if(/*this is player 2*/){
            float player1Theta = gameObject however you access the other objects theta
            float player2Theta = transform.rotation.eulerAngles.y;
        }

		allTheBytes[totalBytes] = BitConverter.GetBytes(player1Location.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player1Location.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player1Location.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player1Location.x)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(player1Location.y)[0]; 
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player1Location.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player1Location.y)[2]; 
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player1Location.y)[3];
		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(player1Location.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player1Location.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player1Location.z)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player1Location.z)[3];

		totalBytes += sizeof(float);

        // add the player1 theta
        allTheBytes[totalBytes] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[0];
        allTheBytes[totalBytes+1] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[1];
        allTheBytes[totalBytes+2] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[2];
        allTheBytes[totalBytes+3] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[3];

        totalBytes += sizeof(float)

        // Player 2 position
		allTheBytes[totalBytes] = BitConverter.GetBytes(player2Location.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player2Location.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player2Location.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player2Location.x)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(player2Location.y)[0]; 
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player2Location.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player2Location.y)[2]; 
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player2Location.y)[3];
		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(player2Location.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(player2Location.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(player2Location.z)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(player2Location.z)[3];

		totalBytes += sizeof(float);

        // player 2 theta
        allTheBytes[totalBytes] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[0];
        allTheBytes[totalBytes+1] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[1];
        allTheBytes[totalBytes+2] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[2];
        allTheBytes[totalBytes+3] = BitConverter.GetBytes(transform.rotation.eulerAngles.y)[3];

        totalBytes += sizeof(float)

		//Send Ball Location
		allTheBytes[totalBytes] = BitConverter.GetBytes(ballLocation.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballLocation.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballLocation.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballLocation.x)[3];
		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ballLocation.y)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballLocation.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballLocation.y)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballLocation.y)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ballLocation.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballLocation.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballLocation.z)[0];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballLocation.z)[1];

		totalBytes += sizeof(float);

		//Send Ball Rotation
		allTheBytes[totalBytes] = BitConverter.GetBytes(ballRotation.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballRotation.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballRotation.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballRotation.x)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ballRotation.y)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballRotation.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballRotation.y)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballRotation.y)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ballRotation.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ballRotation.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ballRotation.z)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ballRotation.z)[3];

		totalBytes += sizeof(float);

		IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

		UdpClient senderUDP = new UdpClient (5009);

		senderUDP.Connect (remoteEP);
		senderUDP.Send (allTheBytes, allTheBytes.Length);

		senderUDP.Close ();

		//udpReceiver.Close ();
    }

	void Update (){

		player1Location = transform.position;
		ballLocation = ball.transform.position;
		ballRotation = ball.transform.rotation.eulerAngles;

		if (serverIp != null && playerIndex == 0) {

			IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

			UdpClient senderUDP = new UdpClient (5009);

			senderUDP.Connect (remoteEP);

			byte[] suhBytes = new byte[1024];
			suhBytes = Encoding.ASCII.GetBytes ("suh dude");
			senderUDP.Send (suhBytes, suhBytes.Length);

			senderUDP.Close ();

		} else if (serverIp != null && playerIndex != 0) {

			sendUpdateToServer ();
			if (!(Mathf.Abs (ballLocation.x - ball.transform.position.x) > 10) || !(Mathf.Abs (ballLocation.y - ball.transform.position.y) > 10) || !(Mathf.Abs (ballLocation.z - ball.transform.position.z) > 10)) { 
				ball.transform.position = ballLocation;
			}
			ball.transform.rotation = Quaternion.Euler (ballRotation);

			//ball.transform.position = Vector3.Lerp (ball.transform.position, ballPositions[locationInt], Time.deltaTime);
			//ball.transform.rotation = Quaternion.Lerp (ball.transform.rotation, Quaternion.Euler (ballRotations[locationInt]), Time.deltaTime);

		}
		//transform.position = Vector3.Lerp (transform.position, player2Locations[locationInt], Time.deltaTime);



	}

	void LateUpdate (){

	}

		

	public int unpack(byte [] receivedBytes)
	{	
		int totalBytes = 0;

		uint someShit =(uint)BitConverter.ToUInt32(receivedBytes, totalBytes);
		totalBytes += sizeof(uint);

		print ("Some Shit " + someShit);

		if (someShit != 1337) {
			playerIndex =  (int)someShit;
			return totalBytes;
		}


		//Player location
		player1Location.x =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		player1Location.y =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		player1Location.z =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);
        
        player1Theta = (float)BitConverter.ToSingle(receivedBytes, totalBytes);
        totalBytes += sizeof(float);

		//Player location
		player2Location.x =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		player2Location.y =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		player2Location.z =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

        player2Theta = (float)BitConverter.ToSingle(receivedBytes, totalBytes);
        totalBytes += sizeof(float);

		player2Locations.Add (player2Location);

		/*Player theta This right here is wrong af bro
		player1Theta =(float)BitConverter.ToInt32(receivedBytes, totalBytes);
		totalBytes += sizeof(float); */

		//Ball Location
		ballLocation.x =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballLocation.y =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballLocation.z =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballPositions.Add (ballLocation);


		//Ball Rotation
		ballRotation.x =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballRotation.y =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballRotation.z =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballRotations.Add (ballRotation);

		locationInt++;

		return totalBytes;

	}

	// This is called whenever data is received
	private void DataReceived(IAsyncResult ar)
	{	

		UdpClient c = (UdpClient)ar.AsyncState;
		IPEndPoint receivedIpEndPoint = new IPEndPoint (IPAddress.Any, 0);


		Byte[] receivedBytes = c.EndReceive (ar, ref receivedIpEndPoint);
		unpack (receivedBytes);

		serverIp = receivedIpEndPoint.Address;

		print ("Server IP " + serverIp);

		c.BeginReceive (DataReceived, ar.AsyncState);

	}

}
