using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;

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
	IPAddress serverIp;

	public List<Vector3> player2Locations = new List<Vector3>();
	public List<Vector3> ballPositions = new List<Vector3>();
	public List<Vector3> ballRotations = new List<Vector3>();

	int locationInt = 0;

	public GameObject ball;

	public int playerIndex;
	public GameObject otherPlayer;

	public Button joinButton;

	public uint playerOneScore;
	public uint playerTwoScore;

	Animator playerAnim;
	Animator opponentAnim;

	public uint animIndex;
	public uint animIndexOther;

	public List<TextMesh> jumboTextObjs = new List<TextMesh>();

	void Start (){
		opponentAnim = otherPlayer.transform.GetChild(0).GetComponent<Animator> ();
		playerAnim = transform.GetChild(1).GetComponent<Animator> ();

		ball = GameObject.Find ("Ball");

		udpReceiver = new UdpClient (5010);

		udpReceiver.JoinMulticastGroup (IPAddress.Parse ("224.0.0.1"));

		udpReceiver.BeginReceive (DataReceived, udpReceiver);

	}

	public void joinServer(){
		
		IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

		UdpClient senderUDP = new UdpClient (5009);

		senderUDP.Connect (remoteEP);

		byte[] suhBytes = new byte[1024];
		suhBytes = Encoding.ASCII.GetBytes ("suh dude");
		senderUDP.Send (suhBytes, suhBytes.Length);

		senderUDP.Close ();

		udpReceiver.BeginReceive (DataReceived, udpReceiver);

	}

	public void Score(){

		IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

		UdpClient senderUDP = new UdpClient (5009);

		senderUDP.Connect (remoteEP);

		byte[] scorePacket = new byte[1024];
		scorePacket = Encoding.ASCII.GetBytes ("score");
		senderUDP.Send (scorePacket, scorePacket.Length);

		senderUDP.Close ();
	}

	public void BallSubmit(){

		IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

		UdpClient senderUDP = new UdpClient (5009);

		senderUDP.Connect (remoteEP);

		byte[] scorePacket = new byte[1024];
		scorePacket = Encoding.ASCII.GetBytes ("ball");
		senderUDP.Send (scorePacket, scorePacket.Length);

		senderUDP.Close ();
	}

	public void sendUpdateToServer(){
		// send the current values for these things to the server
		// on port 5009

		byte[] allTheBytes = new byte[1024];
		int totalBytes = 0;

		if(playerIndex == 1){
			player1Theta = transform.rotation.y;

		} else if(playerIndex == 2){
			player2Theta = transform.rotation.y;
		}


		allTheBytes[totalBytes] = BitConverter.GetBytes(animIndex)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(animIndex)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(animIndex)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(animIndex)[3];

		totalBytes += sizeof(uint);

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
		//If you are player 2 set the location to your own
		if (playerIndex == 2) {
			allTheBytes [totalBytes] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [0];
			allTheBytes [totalBytes + 1] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [1];
			allTheBytes [totalBytes + 2] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [2];
			allTheBytes [totalBytes + 3] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [3];
		} else {
			allTheBytes [totalBytes] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [0];
			allTheBytes [totalBytes + 1] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [1];
			allTheBytes [totalBytes + 2] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [2];
			allTheBytes [totalBytes + 3] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [3];
		}

		totalBytes += sizeof(float);

			// Player 2 position
		allTheBytes[totalBytes]   = BitConverter.GetBytes(player2Location.x)[0];
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
		if (playerIndex == 1) {
			allTheBytes [totalBytes] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [0];
			allTheBytes [totalBytes + 1] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [1];
			allTheBytes [totalBytes + 2] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [2];
			allTheBytes [totalBytes + 3] = BitConverter.GetBytes (otherPlayer.transform.rotation.eulerAngles.y) [3];
		} else {
			allTheBytes [totalBytes] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [0];
			allTheBytes [totalBytes + 1] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [1];
			allTheBytes [totalBytes + 2] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [2];
			allTheBytes [totalBytes + 3] = BitConverter.GetBytes (transform.rotation.eulerAngles.y) [3];
		}

		totalBytes += sizeof(float);

		//Send Ball Location
		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.position.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.position.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.position.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.position.x)[3];
		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.position.y)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.position.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.position.y)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.position.y)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.position.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.position.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.position.z)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.position.z)[3];

		totalBytes += sizeof(float);

		//Send Ball Rotation
		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.rotation.x)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.rotation.x)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.rotation.x)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.rotation.x)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.rotation.y)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.rotation.y)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.rotation.y)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.rotation.y)[3];

		totalBytes += sizeof(float);

		allTheBytes[totalBytes] = BitConverter.GetBytes(ball.transform.rotation.z)[0];
		allTheBytes[totalBytes+1] = BitConverter.GetBytes(ball.transform.rotation.z)[1];
		allTheBytes[totalBytes+2] = BitConverter.GetBytes(ball.transform.rotation.z)[2];
		allTheBytes[totalBytes+3] = BitConverter.GetBytes(ball.transform.rotation.z)[3];

		totalBytes += sizeof(float);

		IPEndPoint remoteEP = new IPEndPoint (serverIp, 5009);

		UdpClient senderUDP = new UdpClient (5009);

		senderUDP.Connect (remoteEP);
		senderUDP.Send (allTheBytes, allTheBytes.Length);

		senderUDP.Close ();

		//udpReceiver.Close ();
	}

	void Update (){

		float currentMax = 0.0f;
		string currentState = "";
		foreach (AnimatorClipInfo animState in playerAnim.GetNextAnimatorClipInfo(0)) {
			if (animState.weight > currentMax) {
				currentMax = animState.weight;
				currentState = animState.clip.name;
			}
		}

			if (currentState == "running_inPlace") {

				animIndex = 1;
				playerAnim.SetFloat ("Movement", 1.0f);

			}

			if (currentState == "left_Strafe") {

				animIndex = 3;
				playerAnim.SetFloat ("strafe", -1.0f);

			}

			if (currentState == "right_Strafe") {

				animIndex = 4;
				playerAnim.SetFloat ("strafe", 1.0f);

			}

			if (currentState == "run_Backwards") {

				animIndex = 2;
				playerAnim.SetFloat ("Movement", -1.0f);

			}

			if (currentState == "jump_inPlace") {

				animIndex = 5;
				playerAnim.SetTrigger ("Jump");

			}

		if (currentState == "Idle_anim") {

			animIndex = 0;
			playerAnim.SetFloat ("Movement", 0.0f);
			playerAnim.SetFloat ("strafe", 0.0f);

		}

		if (serverIp != null && joinButton.interactable == false) {

			joinButton.interactable = true;
		}

		if (playerIndex == 1) {
			player1Location = transform.position;
		}

		if (playerIndex == 2) {
			player2Location = transform.position;
		}


		//ball.transform.position = ballLocation;
		ball.transform.position = Vector3.Lerp (ball.transform.position, ballLocation, Time.deltaTime);
		//}
		ball.transform.rotation = Quaternion.Lerp (ball.transform.rotation, Quaternion.Euler(ballRotation), Time.deltaTime);
	

		if (serverIp != null && playerIndex != 0) {

			sendUpdateToServer ();

			if (playerIndex == 1) {
				otherPlayer.transform.position = Vector3.Lerp (otherPlayer.transform.position, player2Location, Time.deltaTime);
				otherPlayer.transform.rotation = Quaternion.Euler (otherPlayer.transform.rotation.x , player2Theta, otherPlayer.transform.rotation.z);
			} else if (playerIndex == 2) {
				otherPlayer.transform.position = Vector3.Lerp (otherPlayer.transform.position, player1Location, Time.deltaTime);
				otherPlayer.transform.rotation = Quaternion.Euler (otherPlayer.transform.rotation.x, player1Theta, otherPlayer.transform.rotation.z);
			}

			if (animIndexOther == 1) {

				opponentAnim.SetFloat ("Movement", 1.0f);

			}

			if (animIndexOther == 3) {

				opponentAnim.SetFloat ("strafe", -1.0f);

			}

			if (animIndexOther == 4) {

				opponentAnim.SetFloat ("strafe", 1.0f);

			}

			if (animIndexOther == 2) {

				opponentAnim.SetFloat ("Movement", -1.0f);

			}

			if (animIndexOther == 5) {

				opponentAnim.SetTrigger ("Jump");

			}


			foreach (TextMesh text in jumboTextObjs) {
				text.text = ("Green : " + playerOneScore.ToString() + " Purple : " + playerTwoScore.ToString());

			}

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

		animIndex = (uint)BitConverter.ToUInt32 (receivedBytes, totalBytes);
		totalBytes += sizeof(uint);

		animIndexOther = (uint)BitConverter.ToUInt32 (receivedBytes, totalBytes);
		totalBytes += sizeof(uint);

		playerOneScore = (uint)BitConverter.ToUInt32 (receivedBytes, totalBytes);
		totalBytes += sizeof(uint);

		playerTwoScore = (uint)BitConverter.ToUInt32 (receivedBytes, totalBytes);
		totalBytes += sizeof(uint);

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

		//player2Locations.Add (player2Location);

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

		//ballPositions.Add (ballLocation);


		//Ball Rotation
		ballRotation.x =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballRotation.y =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		ballRotation.z =(float)BitConverter.ToSingle(receivedBytes, totalBytes);
		totalBytes += sizeof(float);

		//ballRotations.Add (ballRotation);

		locationInt++;

		return totalBytes;

	}

	// This is called whenever data is received
	private void DataReceived(IAsyncResult ar)
	{	
		UdpClient c = (UdpClient)ar.AsyncState;
		IPEndPoint receivedIpEndPoint = new IPEndPoint (IPAddress.Any, 0);

		Byte[] receivedBytes = c.EndReceive (ar, ref receivedIpEndPoint);

		if (serverIp != null) {
			unpack (receivedBytes);
		} else {
			serverIp = receivedIpEndPoint.Address;
			return;
		}

		print ("Server IP " + serverIp);

		c.BeginReceive (DataReceived, ar.AsyncState);

	}

}